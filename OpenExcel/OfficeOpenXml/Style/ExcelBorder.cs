﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenExcel.OfficeOpenXml.Internal;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OpenExcel.OfficeOpenXml.Style
{
    // TODO: make an Enum for border styles

    public class ExcelBorder
    {
        private DocumentStyles _styles;
        private IStylable _stylable;
        private uint? _borderId;
        internal Border BorderObject { get; set; }

        internal ExcelBorder(IStylable stylable, DocumentStyles styles, uint? borderId)
        {
            _stylable = stylable;
            _styles = styles;
            _borderId = borderId;
            if (_borderId != null)
                BorderObject = (Border)_styles.GetBorder(_borderId.Value).CloneNode(true);
            else
                BorderObject = new Border();
        }

        public ExcelBorderStyleValues LeftStyle
        {
            get
            {
                return GetBorderStyle(BorderObject.LeftBorder);
            }
            set
            {
                SetBorderStyle(BorderObject.LeftBorder, value);
            }
        }

        public ExcelBorderStyleValues RightStyle
        {
            get
            {
                return GetBorderStyle(BorderObject.RightBorder);
            }
            set
            {
                SetBorderStyle(BorderObject.RightBorder, value);
            }
        }

        public ExcelBorderStyleValues TopStyle
        {
            get
            {
                return GetBorderStyle(BorderObject.TopBorder);
            }
            set
            {
                SetBorderStyle(BorderObject.TopBorder, value);
            }
        }

        public ExcelBorderStyleValues BottomStyle
        {
            get
            {
                return GetBorderStyle(BorderObject.BottomBorder);
            }
            set
            {
                SetBorderStyle(BorderObject.BottomBorder, value);
            }
        }

        private ExcelBorderStyleValues GetBorderStyle(BorderPropertiesType b)
        {
            return (ExcelBorderStyleValues)b.Style.Value;
        }

        private void SetBorderStyle(BorderPropertiesType b, ExcelBorderStyleValues val)
        {
            b.Style = (BorderStyleValues)val;
            if (_stylable != null)
                _stylable.Style.Border = this;
        }
    }
}
