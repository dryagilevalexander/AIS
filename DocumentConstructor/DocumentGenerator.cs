using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using JustificationValues = DocumentFormat.OpenXml.Wordprocessing.JustificationValues;
using Justification = DocumentFormat.OpenXml.Wordprocessing.Justification;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Style = DocumentFormat.OpenXml.Wordprocessing.Style;
using StyleValues = DocumentFormat.OpenXml.Wordprocessing.StyleValues;

namespace OpenXML
{
    public class DocumentGenerator
    {
        public void CreateContract(string filePath, Contract contract)
        {
            //Создаем новый документ
            WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            //Создаем корень документа
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
            //Создаем Body
            Body body = mainPart.Document.AppendChild(new Body());

            //Определяем стили используемые в документе
            StyleDefinitionsPart part = mainPart.StyleDefinitionsPart;
            if (part == null) part = AddStylesPartToPackage(wordDoc);
            CreateAndAddNoSpaceParagraphStyle(part, "a3");

            //Определяем уровни нумерации. Уровней нумерации 9
            NumberingDefinitionsPart numberingDefinitionsPart = mainPart.AddNewPart<NumberingDefinitionsPart>("rId1");
            GenerateNumberingDefinitionsPart1Content(numberingDefinitionsPart);

            List<Condition> conditions = contract.Conditions;
            foreach(var condit in conditions)
            {
                if(condit.TypeOfCondition==1)
                {
                    createParagraph(wordDoc, "a3", "24", false, 0, 0, "center", true, condit.Name);
                    CreateDateAndPlaceTable(wordDoc, contract.PlaceOfContract, "__.__.202_");
                    createParagraph(wordDoc, "a3", "24", false, 0, 0, "center", false, "");
                }

                if (condit.TypeOfCondition == 2)
                {
                    createParagraph(wordDoc, "a3", "24", false, 0, 0, "both", false, condit.Text);
                    createParagraph(wordDoc, "a3", "24", false, 0, 0, "center", false, "");
                }

                if (condit.TypeOfCondition == 3)
                {
                    createParagraph(wordDoc, "a3", "24", true, 0, 1, "center", true, condit.Name);
                    if(condit.SubConditions!= null)
                    {
                        foreach (var item in condit.SubConditions)
                        {
                            createParagraph(wordDoc, "a3", "24", true, 1, 1, "both", false, item.Text);
                            if (item.SubConditionParagraphs != null)
                            {
                                foreach (var paragraph in item.SubConditionParagraphs)
                                {
                                    createParagraph(wordDoc, "a3", "24", true, 2, 1, "both", false, paragraph.Text);
                                }
                            }
                        }
                    }
                }
                createParagraph(wordDoc, "a3", "24", false, 0, 0, "center", false, "");
            }

            createParagraph(wordDoc, "a3", "24", true, 0, 1, "center", true, "Реквизиты");
            CreateTable(wordDoc, contract.CustomerProp, contract.ExecutorProp);
            //Закрываем документ (автосохранение включено)
            wordDoc.Close();

            //Заменяем теги значениями из модели контракта
            ReplacingTags(contract, filePath);
        }

        //Метод замены тегов значениями из модели контракта
        public void ReplacingTags(Contract contract, string filePath)
        {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
                {
                //Получаем текст (вместе с разметкой) из MainDocumentPart
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                string contractType = "";
                string contractName = "";
                string baseOfContract = "";
                string paragraphBaseOfContract = "";
                string executor = "";

                //Получаем тип договора
                switch (contract.ContractType)
                {
                    case 1:
                        contractType = "подряда";
                        break;
                    case 2:
                        contractType = "оказания услуг";
                        break;
                    case 3:
                        contractType = "поставки";
                        break;
                    case 4:
                        contractType = "аренды";
                        break;
                }

                switch (contract.ContractType)
                {
                    case 1:
                        executor = "Подрядчик";
                        break;
                    case 2:
                        executor = "Исполнитель";
                        break;
                    case 3:
                        executor = "Поставщик";
                        break;
                    case 4:
                        executor = "Арендатор";
                        break;
                }

                //Получаем пункт основания заключения контракта (для 44-ФЗ)
                if (contract.RegulationType == 3)
                {
                    switch (contract.RegulationParagraph)
                    {
                        case 1:
                            paragraphBaseOfContract = "п. 4 ст. 93 ";
                            break;
                        case 2:
                            paragraphBaseOfContract = "п. 8 ст. 93 ";
                            break;
                    }
                }

                //Получаем фактическое наименование контракта и основание заключения
                switch (contract.RegulationType)
                {
                    case 1:
                        contractName = "Договор";
                        break;
                    case 2:
                        contractName = "Договор";
                        baseOfContract = "на основании федерального закона \"О закупках товаров, работ, услуг отдельными видами юридических лиц\" от 18.07.2011 N 223-ФЗ,";
                        break;
                    case 3:
                        contractName = "Контракт";
                        baseOfContract = "на основании " + paragraphBaseOfContract + "федерального закона \"О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд\" от 05.04.2013 N 44-ФЗ,";
                        break;
                }

                docText = docText.Replace("договор", contractName);
                docText = docText.Replace("contractType", contractType);
                docText = docText.Replace("customerName", contract.Customer.Name);
                docText = docText.Replace("executorName", contract.Executor.Name);
                docText = docText.Replace("customerDirectorNameR", contract.Customer.DirectorNameR);
                docText = docText.Replace("executorDirectorNameR", contract.Executor.DirectorNameR);
                docText = docText.Replace("baseOfContract", baseOfContract);
                docText = docText.Replace("subjectOfContract", contract.SubjectOfContract);
                docText = docText.Replace("dateEnd", contract.DateEnd);
                docText = docText.Replace("customerDirectorTypeNameR", contract.Customer.DirectorType.NameR);
                docText = docText.Replace("executorDirectorTypeNameR", contract.Executor.DirectorType.NameR);
                docText = docText.Replace("executor", executor);


                //Записываем изменения
                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                }
        }


        //Метод создания абзаца
        //Параметры:
        //wordDoc - ссылка на документ
        //styleId - id стиля применяемого к абзацу
        //paragraphfontSize - размер шрифта
        //isNumbering - применять ли нумерацию
        //numLevelReference - уровень нумерации (1, 1.1., 1.1.1 и т.д.)
        //numId - id группировки нумерации, для всех одинаковых id нумерация будет генерироваться последовательно, при numLevelReference=0 (1,2,3,4)
        //justificationValue - горизонтальное выравнивание
        //txt - текст абзаца

        public void createParagraph(WordprocessingDocument wordDoc, string styleId, string paragraphfontSize, bool isNumbering, int numLevelReference, int numId, string justificationValue, bool isBold, string txt)
        {
            //Получаем корень документа
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            //Получаем Body
            Body body = mainPart.Document.Body;
            
            //Создаем абзац и его свойства
            Paragraph para = body.AppendChild(new Paragraph());
            ParagraphProperties paragraphProperties = new ParagraphProperties();


            //Определяем шрифты
            ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();
            RunFonts runFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize1 = new FontSize() { Val = paragraphfontSize };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = paragraphfontSize };
            paragraphMarkRunProperties.Append(fontSize1);
            paragraphMarkRunProperties.Append(fontSizeComplexScript1);
            paragraphMarkRunProperties.Append(runFonts);
            //если шрифт жирный
            if(isBold)
            {
                Bold bold = new Bold();
                paragraphMarkRunProperties.Append(bold);
            }

            paragraphProperties.Append(paragraphMarkRunProperties);
 
            if (isNumbering)
            {
            //Определяем нумерацию
            NumberingProperties numberingProperties = new NumberingProperties();
            NumberingLevelReference numberingLevelReference = new NumberingLevelReference() { Val = numLevelReference };
            NumberingId numberingId = new NumberingId() { Val = numId };
            numberingProperties.Append(numberingLevelReference);
            numberingProperties.Append(numberingId);
            paragraphProperties.Append(numberingProperties);
            }

            Justification justification = new Justification();
            switch (justificationValue)
            {
                case "left":
                    justification.Val = JustificationValues.Left;
                break;
                case "center":
                    justification.Val = JustificationValues.Center;
                break;
                case "right":
                    justification.Val = JustificationValues.Right;
                break;
                case "both":
                    justification.Val = JustificationValues.Both;
                break;
            }
            paragraphProperties.Append(justification);

            if (paragraphProperties.ParagraphStyleId == null) paragraphProperties.ParagraphStyleId = new ParagraphStyleId();
            paragraphProperties.ParagraphStyleId.Val = styleId;

            //Добавляем сформированные свойства в абзац
            para.Append(paragraphProperties);

            //Добавляем текст с определенными свойствами в абзац
            Run run = para.AppendChild(new Run());
            RunProperties runProperties = new RunProperties();
            //Тип и размер шрифта текста
            RunFonts runFonts1 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize2 = new FontSize() { Val = paragraphfontSize };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = paragraphfontSize };
            runProperties.Append(runFonts1);
            runProperties.Append(fontSize2);
            runProperties.Append(fontSizeComplexScript2);
            
            //если шрифт жирный
            if (isBold)
            {
                Bold bold = new Bold();
                runProperties.Append(bold);
            }

            run.Append(runProperties);
            run.AppendChild(new Text(txt));
        }


        public static Paragraph GetParagraph(WordprocessingDocument wordDoc, string styleId, string paragraphfontSize, bool isNumbering, int numId, string justificationValue, string txt)
        {
            //Получаем корень документа
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            //Получаем Body
            Body body = mainPart.Document.Body;

            //Создаем абзац и его свойства
            Paragraph para = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();


            //Определяем шрифты
            ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();
            RunFonts runFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize1 = new FontSize() { Val = paragraphfontSize };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = paragraphfontSize };
            paragraphMarkRunProperties.Append(fontSize1);
            paragraphMarkRunProperties.Append(fontSizeComplexScript1);
            paragraphMarkRunProperties.Append(runFonts);
            paragraphProperties.Append(paragraphMarkRunProperties);

            if (isNumbering == true)
            {
                //Определяем нумерацию
                NumberingProperties numberingProperties = new NumberingProperties();
                NumberingLevelReference numberingLevelReference = new NumberingLevelReference() { Val = 0 };
                NumberingId numberingId = new NumberingId() { Val = numId };
                numberingProperties.Append(numberingLevelReference);
                numberingProperties.Append(numberingId);
                paragraphProperties.Append(numberingProperties);
            }

            Justification justification = new Justification();
            switch (justificationValue)
            {
                case "left":
                    justification.Val = JustificationValues.Left;
                    break;
                case "center":
                    justification.Val = JustificationValues.Center;
                    break;
                case "right":
                    justification.Val = JustificationValues.Right;
                    break;
            }
            paragraphProperties.Append(justification);

            if (paragraphProperties.ParagraphStyleId == null) paragraphProperties.ParagraphStyleId = new ParagraphStyleId();
            paragraphProperties.ParagraphStyleId.Val = styleId;

            //Добавляем сформированные свойства в абзац
            para.Append(paragraphProperties);


            //Добавляем текст с определенными свойствами в абзац
            Run run = para.AppendChild(new Run());
            RunProperties runProperties = new RunProperties();
            //Тип и размер шрифта текста
            RunFonts runFonts1 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize2 = new FontSize() { Val = paragraphfontSize };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = paragraphfontSize };
            runProperties.Append(runFonts1);
            runProperties.Append(fontSize2);
            runProperties.Append(fontSizeComplexScript2);
            run.Append(runProperties);
            run.AppendChild(new Text(txt));
            return para;
        }

        //Метод создания стиля "No Spacing"
        public static void CreateAndAddNoSpaceParagraphStyle(StyleDefinitionsPart styleDefinitionsPart, string styleId)
        {
            // Access the root element of the styles part.
            Styles styles = styleDefinitionsPart.Styles;
            if (styles == null)
            {
                styleDefinitionsPart.Styles = new Styles();
                styleDefinitionsPart.Styles.Save();
            }


            Style style = new Style() { Type = StyleValues.Paragraph, StyleId = styleId };
            StyleName styleName = new StyleName() { Val = "No Spacing" };
            UIPriority uIPriority = new UIPriority() { Val = 1 };
            PrimaryStyle primaryStyle = new PrimaryStyle();

            StyleParagraphProperties styleParagraphProperties = new StyleParagraphProperties();
            SpacingBetweenLines spacingBetweenLines = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties.Append(spacingBetweenLines);

            style.Append(styleName);
            style.Append(uIPriority);
            style.Append(primaryStyle);
            style.Append(styleParagraphProperties);

            // Add the style to the styles part.
            styles.Append(style);
        }

        //Метод добавления раздела стилей в документ
        public static StyleDefinitionsPart AddStylesPartToPackage(WordprocessingDocument doc)
        {
            StyleDefinitionsPart part;
            part = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            Styles root = new Styles();
            root.Save(part);
            return part;
        }

        //Метод добавления однострочной таблицы (шапкаб реквизиты)
        public static void CreateTable(WordprocessingDocument doc, Dictionary<string, string> customerProp, Dictionary<string, string> executorProp)
        {
            //Получаем корень документа
            MainDocumentPart mainPart = doc.MainDocumentPart;
            
            //Получаем Body
            Body body = mainPart.Document.Body;

            //Создаем таблицу и ее свойства
            Table table = body.AppendChild(new Table());

            TableProperties tableProperties = new TableProperties();
            TableWidth tableWidth = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableLook tableLook = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties.Append(tableWidth);
            tableProperties.Append(tableLook);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "4672" };
            GridColumn gridColumn2 = new GridColumn() { Width = "4673" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);

            TableRow tableRow1 = new TableRow();

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4672", Type = TableWidthUnitValues.Dxa };

            tableCellProperties1.Append(tableCellWidth1);
            foreach(var prop in customerProp)
            { 
            Paragraph paragraph = GetParagraph(doc, "a3", "24", false, 0, "left", prop.Key + " " + prop.Value);
            tableCell1.Append(paragraph);
            }
            tableCell1.Append(tableCellProperties1);


            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "4673", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);
            foreach (var prop in executorProp)
            {
                Paragraph paragraph = GetParagraph(doc, "a3", "24", false, 0, "left", prop.Key + " " + prop.Value);
                tableCell2.Append(paragraph);
            }
            tableCell2.Append(tableCellProperties2);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);

            table.Append(tableProperties);
            table.Append(tableGrid1);
            table.Append(tableRow1);
        }

        //Метод добавления однострочной таблицы (место, дата)
        public static void CreateDateAndPlaceTable(WordprocessingDocument doc, string place, string dateTemplate)
        {
            //Получаем корень документа
            MainDocumentPart mainPart = doc.MainDocumentPart;

            //Получаем Body
            Body body = mainPart.Document.Body;

            //Создаем таблицу и ее свойства
            Table table = body.AppendChild(new Table());

            TableProperties tableProperties = new TableProperties();
            TableWidth tableWidth = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableLook tableLook = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties.Append(tableWidth);
            tableProperties.Append(tableLook);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "4672" };
            GridColumn gridColumn2 = new GridColumn() { Width = "4673" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);

            TableRow tableRow1 = new TableRow();

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4672", Type = TableWidthUnitValues.Dxa };

            tableCellProperties1.Append(tableCellWidth1);
                Paragraph paragraph1 = GetParagraph(doc, "a3", "24", false, 0, "left", place);
                tableCell1.Append(paragraph1);
            tableCell1.Append(tableCellProperties1);


            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "4673", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);
            Paragraph paragraph2 = GetParagraph(doc, "a3", "24", false, 0, "right", dateTemplate);
            tableCell2.Append(paragraph2);
            tableCell2.Append(tableCellProperties2);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);

            table.Append(tableProperties);
            table.Append(tableGrid1);
            table.Append(tableRow1);
        }

        //Метод определения уровней нумерации. Уровней нумерации 9
        private void GenerateNumberingDefinitionsPart1Content(NumberingDefinitionsPart numberingDefinitionsPart1)
        {
            Numbering numbering1 = new Numbering() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 w16se wp14" } };
            numbering1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            numbering1.AddNamespaceDeclaration("cx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
            numbering1.AddNamespaceDeclaration("cx1", "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
            numbering1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            numbering1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            numbering1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            numbering1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            numbering1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            numbering1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            numbering1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            numbering1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            numbering1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            numbering1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            numbering1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            numbering1.AddNamespaceDeclaration("w16se", "http://schemas.microsoft.com/office/word/2015/wordml/symex");
            numbering1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            numbering1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            numbering1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            numbering1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = 0 };
            abstractNum1.SetAttribute(new OpenXmlAttribute("w15", "restartNumberingAfterBreak", "http://schemas.microsoft.com/office/word/2012/wordml", "0"));
            Nsid nsid1 = new Nsid() { Val = "7EEA2E5F" };
            MultiLevelType multiLevelType1 = new MultiLevelType() { Val = MultiLevelValues.Multilevel };
            TemplateCode templateCode1 = new TemplateCode() { Val = "E49CBF64" };

            Level level1 = new Level() { LevelIndex = 0 };
            StartNumberingValue startNumberingValue1 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat1 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            LevelText levelText1 = new LevelText() { Val = "%1." };
            LevelJustification levelJustification1 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties1 = new PreviousParagraphProperties();
            Indentation indentation2 = new Indentation() { Start = "720", Hanging = "360" };

            previousParagraphProperties1.Append(indentation2);

            NumberingSymbolRunProperties numberingSymbolRunProperties1 = new NumberingSymbolRunProperties();
            RunFonts runFonts2 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties1.Append(runFonts2);

            level1.Append(startNumberingValue1);
            level1.Append(numberingFormat1);
            level1.Append(levelText1);
            level1.Append(levelJustification1);
            level1.Append(previousParagraphProperties1);
            level1.Append(numberingSymbolRunProperties1);

            Level level2 = new Level() { LevelIndex = 1 };
            StartNumberingValue startNumberingValue2 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat2 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle1 = new IsLegalNumberingStyle();
            LevelText levelText2 = new LevelText() { Val = "%1.%2." };
            LevelJustification levelJustification2 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties2 = new PreviousParagraphProperties();
            Indentation indentation3 = new Indentation() { Start = "1080", Hanging = "360" };

            previousParagraphProperties2.Append(indentation3);

            NumberingSymbolRunProperties numberingSymbolRunProperties2 = new NumberingSymbolRunProperties();
            RunFonts runFonts3 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties2.Append(runFonts3);

            level2.Append(startNumberingValue2);
            level2.Append(numberingFormat2);
            level2.Append(isLegalNumberingStyle1);
            level2.Append(levelText2);
            level2.Append(levelJustification2);
            level2.Append(previousParagraphProperties2);
            level2.Append(numberingSymbolRunProperties2);

            Level level3 = new Level() { LevelIndex = 2 };
            StartNumberingValue startNumberingValue3 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat3 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle2 = new IsLegalNumberingStyle();
            LevelText levelText3 = new LevelText() { Val = "%1.%2.%3." };
            LevelJustification levelJustification3 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties3 = new PreviousParagraphProperties();
            Indentation indentation4 = new Indentation() { Start = "1800", Hanging = "720" };

            previousParagraphProperties3.Append(indentation4);

            NumberingSymbolRunProperties numberingSymbolRunProperties3 = new NumberingSymbolRunProperties();
            RunFonts runFonts4 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties3.Append(runFonts4);

            level3.Append(startNumberingValue3);
            level3.Append(numberingFormat3);
            level3.Append(isLegalNumberingStyle2);
            level3.Append(levelText3);
            level3.Append(levelJustification3);
            level3.Append(previousParagraphProperties3);
            level3.Append(numberingSymbolRunProperties3);

            Level level4 = new Level() { LevelIndex = 3 };
            StartNumberingValue startNumberingValue4 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat4 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle3 = new IsLegalNumberingStyle();
            LevelText levelText4 = new LevelText() { Val = "%1.%2.%3.%4." };
            LevelJustification levelJustification4 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties4 = new PreviousParagraphProperties();
            Indentation indentation5 = new Indentation() { Start = "2160", Hanging = "720" };

            previousParagraphProperties4.Append(indentation5);

            NumberingSymbolRunProperties numberingSymbolRunProperties4 = new NumberingSymbolRunProperties();
            RunFonts runFonts5 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties4.Append(runFonts5);

            level4.Append(startNumberingValue4);
            level4.Append(numberingFormat4);
            level4.Append(isLegalNumberingStyle3);
            level4.Append(levelText4);
            level4.Append(levelJustification4);
            level4.Append(previousParagraphProperties4);
            level4.Append(numberingSymbolRunProperties4);

            Level level5 = new Level() { LevelIndex = 4 };
            StartNumberingValue startNumberingValue5 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat5 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle4 = new IsLegalNumberingStyle();
            LevelText levelText5 = new LevelText() { Val = "%1.%2.%3.%4.%5." };
            LevelJustification levelJustification5 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties5 = new PreviousParagraphProperties();
            Indentation indentation6 = new Indentation() { Start = "2880", Hanging = "1080" };

            previousParagraphProperties5.Append(indentation6);

            NumberingSymbolRunProperties numberingSymbolRunProperties5 = new NumberingSymbolRunProperties();
            RunFonts runFonts6 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties5.Append(runFonts6);

            level5.Append(startNumberingValue5);
            level5.Append(numberingFormat5);
            level5.Append(isLegalNumberingStyle4);
            level5.Append(levelText5);
            level5.Append(levelJustification5);
            level5.Append(previousParagraphProperties5);
            level5.Append(numberingSymbolRunProperties5);

            Level level6 = new Level() { LevelIndex = 5 };
            StartNumberingValue startNumberingValue6 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat6 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle5 = new IsLegalNumberingStyle();
            LevelText levelText6 = new LevelText() { Val = "%1.%2.%3.%4.%5.%6." };
            LevelJustification levelJustification6 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties6 = new PreviousParagraphProperties();
            Indentation indentation7 = new Indentation() { Start = "3240", Hanging = "1080" };

            previousParagraphProperties6.Append(indentation7);

            NumberingSymbolRunProperties numberingSymbolRunProperties6 = new NumberingSymbolRunProperties();
            RunFonts runFonts7 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties6.Append(runFonts7);

            level6.Append(startNumberingValue6);
            level6.Append(numberingFormat6);
            level6.Append(isLegalNumberingStyle5);
            level6.Append(levelText6);
            level6.Append(levelJustification6);
            level6.Append(previousParagraphProperties6);
            level6.Append(numberingSymbolRunProperties6);

            Level level7 = new Level() { LevelIndex = 6 };
            StartNumberingValue startNumberingValue7 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat7 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle6 = new IsLegalNumberingStyle();
            LevelText levelText7 = new LevelText() { Val = "%1.%2.%3.%4.%5.%6.%7." };
            LevelJustification levelJustification7 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties7 = new PreviousParagraphProperties();
            Indentation indentation8 = new Indentation() { Start = "3960", Hanging = "1440" };

            previousParagraphProperties7.Append(indentation8);

            NumberingSymbolRunProperties numberingSymbolRunProperties7 = new NumberingSymbolRunProperties();
            RunFonts runFonts8 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties7.Append(runFonts8);

            level7.Append(startNumberingValue7);
            level7.Append(numberingFormat7);
            level7.Append(isLegalNumberingStyle6);
            level7.Append(levelText7);
            level7.Append(levelJustification7);
            level7.Append(previousParagraphProperties7);
            level7.Append(numberingSymbolRunProperties7);

            Level level8 = new Level() { LevelIndex = 7 };
            StartNumberingValue startNumberingValue8 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat8 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle7 = new IsLegalNumberingStyle();
            LevelText levelText8 = new LevelText() { Val = "%1.%2.%3.%4.%5.%6.%7.%8." };
            LevelJustification levelJustification8 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties8 = new PreviousParagraphProperties();
            Indentation indentation9 = new Indentation() { Start = "4320", Hanging = "1440" };

            previousParagraphProperties8.Append(indentation9);

            NumberingSymbolRunProperties numberingSymbolRunProperties8 = new NumberingSymbolRunProperties();
            RunFonts runFonts9 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties8.Append(runFonts9);

            level8.Append(startNumberingValue8);
            level8.Append(numberingFormat8);
            level8.Append(isLegalNumberingStyle7);
            level8.Append(levelText8);
            level8.Append(levelJustification8);
            level8.Append(previousParagraphProperties8);
            level8.Append(numberingSymbolRunProperties8);

            Level level9 = new Level() { LevelIndex = 8 };
            StartNumberingValue startNumberingValue9 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat9 = new NumberingFormat() { Val = NumberFormatValues.Decimal };
            IsLegalNumberingStyle isLegalNumberingStyle8 = new IsLegalNumberingStyle();
            LevelText levelText9 = new LevelText() { Val = "%1.%2.%3.%4.%5.%6.%7.%8.%9." };
            LevelJustification levelJustification9 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties9 = new PreviousParagraphProperties();
            Indentation indentation10 = new Indentation() { Start = "5040", Hanging = "1800" };

            previousParagraphProperties9.Append(indentation10);

            NumberingSymbolRunProperties numberingSymbolRunProperties9 = new NumberingSymbolRunProperties();
            RunFonts runFonts10 = new RunFonts() { Hint = FontTypeHintValues.Default };

            numberingSymbolRunProperties9.Append(runFonts10);

            level9.Append(startNumberingValue9);
            level9.Append(numberingFormat9);
            level9.Append(isLegalNumberingStyle8);
            level9.Append(levelText9);
            level9.Append(levelJustification9);
            level9.Append(previousParagraphProperties9);
            level9.Append(numberingSymbolRunProperties9);

            abstractNum1.Append(nsid1);
            abstractNum1.Append(multiLevelType1);
            abstractNum1.Append(templateCode1);
            abstractNum1.Append(level1);
            abstractNum1.Append(level2);
            abstractNum1.Append(level3);
            abstractNum1.Append(level4);
            abstractNum1.Append(level5);
            abstractNum1.Append(level6);
            abstractNum1.Append(level7);
            abstractNum1.Append(level8);
            abstractNum1.Append(level9);

            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = 1 };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = 0 };

            numberingInstance1.Append(abstractNumId1);

            numbering1.Append(abstractNum1);
            numbering1.Append(numberingInstance1);

            numberingDefinitionsPart1.Numbering = numbering1;
        }
    }
}

