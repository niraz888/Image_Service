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
    public class ConvertColorMainWindow :IValueConverter 
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
            bool isConnect = (bool)value;
            if (isConnect)
            {
                return "Transparent";
            }
            else
            {
                return "Gray";
            }
        }
           public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
