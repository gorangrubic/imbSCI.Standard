using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace imbSCI.Elecronics.Resistors
{
    public class ResistorColorCode
    {

        private static Object _colorCodeDictionary_lock = new Object();
        private static Dictionary<Int32, Color> _colorCodeDictionary;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<Int32, Color> colorCodeDictionary
        {
            get
            {
                if (_colorCodeDictionary == null)
                {
                    lock (_colorCodeDictionary_lock)
                    {

                        if (_colorCodeDictionary == null)
                        {
                            _colorCodeDictionary = new Dictionary<Int32, Color>();
                            _colorCodeDictionary.Add(0, Color.Black);
                            _colorCodeDictionary.Add(1, Color.Brown);
                            _colorCodeDictionary.Add(2, Color.Red);
                            _colorCodeDictionary.Add(3, Color.Orange);
                            _colorCodeDictionary.Add(4, Color.Yellow);
                            _colorCodeDictionary.Add(5, Color.Green);
                            _colorCodeDictionary.Add(6, Color.Blue);
                            _colorCodeDictionary.Add(7, Color.Violet);
                            _colorCodeDictionary.Add(8, Color.Gray);
                            _colorCodeDictionary.Add(9, Color.White);
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _colorCodeDictionary;
            }
        }


        public Color DigitToColor(Int32 digit)
        {
            switch (digit)
            {
                case 0:
                    break;

            }
        }

    }
}
