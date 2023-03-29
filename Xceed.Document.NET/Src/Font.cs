﻿/***************************************************************************************
 
   DocX – DocX is the community edition of Xceed Words for .NET
 
   Copyright (C) 2009-2022 Xceed Software Inc.
 
   This program is provided to you under the terms of the XCEED SOFTWARE, INC.
   COMMUNITY LICENSE AGREEMENT (for non-commercial use) as published at 
   https://github.com/xceedsoftware/DocX/blob/master/license.md
 
   For more features and fast professional support,
   pick up Xceed Words for .NET at https://xceed.com/xceed-words-for-net/
 
  *************************************************************************************/


using System;

namespace Xceed.Document.NET
{
  public sealed class Font
  {
    public Font( string name )
    {
      if( string.IsNullOrEmpty( name ) )
        throw new ArgumentNullException( nameof( name ) );

      Name = name;
    }

    public string Name
    {
      get;
      private set;
    }

    public override string ToString()
    {
      return Name;
    }

    public static bool operator ==( Font x, Font y )
    {
      return object.Equals( x, y );
    }

    public static bool operator !=( Font x, Font y )
    {
      return !object.Equals( x, y );
    }

    public override bool Equals( object obj )
    {
      if( obj == null )
        return false;
      if( ReferenceEquals( obj, this ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      Font rhs = obj as Font;
      return this.Name == rhs.Name;
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }
  }
}
