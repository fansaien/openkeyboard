﻿using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpenKeyboard
{
    public class vButton : Button
    {
        public KeyboardCommand KBCommand;

        private string defaultText = "";
        private string shiftTextValue = "";

        // Dependency Property
        public static readonly DependencyProperty ShiftTextProperty = DependencyProperty.Register("ShiftText", typeof(string), typeof(vButton), new FrameworkPropertyMetadata(""));
        public string ShiftText
        {
            get { return (string)this.GetValue(ShiftTextProperty); }
            set 
            { 
                this.SetValue(ShiftTextProperty, value);
                if (string.IsNullOrEmpty(shiftTextValue))
                {
                    shiftTextValue = value;
                }
            }
        }//prop

        public string Title
        {
            set
            {
                if (value.StartsWith("\\u"))
                {
                    defaultText = parseUnicode(value);
                }
                else
                { 
                    defaultText = value;
                    Content = value;
                } 
            }
        }//prop

        private string parseUnicode(string txt)
        {
            int pos = 0;
            string tmp = "", final = "";

            //Check if only one unicode escaped character in the string.
            if (txt.Length == 6)
            {
                Content = (char)Int32.Parse(txt.Substring(2), System.Globalization.NumberStyles.HexNumber);
                return Content.ToString();
            }//if

            //More then one possible unicode characters
            while (pos < txt.Length)
            {
                //If not unicode escaped, add to final
                if (txt[pos] != '\\') { final += txt[pos]; pos++; }
                else
                { //unicode escaped, Parse it.
                    tmp = txt.Substring(pos + 2, 4);
                    final += (char)Int32.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                    pos += 6;
                }//if
            }//while

            Content = final;
            return final;
        }//func

        public void RefreshButton(bool toUpper)
        {


            if (!string.IsNullOrEmpty(ShiftText))
            {
                Content = toUpper ? shiftTextValue : defaultText;
                ShiftText = toUpper ? defaultText : shiftTextValue;
                return;
            }

            var txt = Content as string;

            if (txt.Length == 1)
                Content = toUpper ? txt.ToUpper() : txt.ToLower();

            
        }

        static vButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(vButton), new FrameworkPropertyMetadata(typeof(vButton)));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnApplyTemplate();
        }
    }//cls
}//ns
