using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.View
{
    class ColorForLogConvertor : IValueConverter
    {
        /// <summary>
        /// convert.
        /// check first if the target is brush -> if now than throw exeception.
        /// else convert the type of message to the according color.
        /// </summary>
        /// <param name="value">message</param>
        /// <param name="target">brush</param>
        /// <param name="param"></param>
        /// <param name="info"></param>
        /// <returns>string represent the color</returns>
        public object Convert(Object value, Type target, object param, CultureInfo info)
        {
            if (target.Name != "Brush")
            {
                throw new Exception("Error - should be a brush");
            }
            MessageTypeEnum msg = (MessageTypeEnum)value;
            if (msg == MessageTypeEnum.WARNING)
            {
                return "Yellow";
            } else if (msg == MessageTypeEnum.INFO)
            {
                return "Green";
            } else
            {
                return "Red";
            }
        }
        public object ConvertBack(object value, Type target, object param, CultureInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
