﻿/*
' Copyright (c) 2010-2012 Christoc.com
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
' 
*/
using Christoc.Com.Modules.Ladder.Data;
using DotNetNuke.Common.Utilities;

namespace Christoc.Com.Modules.Ladder.Components
{
    public class FieldController
    {

        /* create field workflow */

        //save field
        public Field SaveField(Field f)
        {
            f = f.FieldId > 0 ? UpdateField(f) : CreateField(f);
            return f;
        }

        private static Field CreateField(Field f)
        {
            f.FieldId = DataProvider.Instance().AddField(f);
            return f;
        }

        //update field
        private static Field UpdateField(Field f)
        {
            DataProvider.Instance().UpdateField(f);
            return f;
        }

        public static Field GetField(string fieldIdentifier)
        {
            var f = CBO.FillObject<Field>(DataProvider.Instance().GetField(fieldIdentifier));
            if(f!=null)
            {
                return f;
            }
            return null;
        }
    }
}