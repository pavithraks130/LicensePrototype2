using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace License.MetCalDesktop.Common
{
    internal static class DataValidations
    {
        public static bool IsValidCardDetails(TextBox cardNumber, ComboBox slectedMonth, TextBox txtCVV,
            ComboBox selectedYear)
        {
            if (cardNumber.Text.Length == Constants.CARD_NUMBER_LENGTH && slectedMonth.SelectedItem is int &&
                txtCVV.Text.Length == Constants.CARD_CVV_LENGTH && txtCVV.Text != string.Empty
                && selectedYear.SelectedItem != null)
            {
                return true;
            }
            return false;
        }

        public static bool IsValidEmailId(string emailID)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailID);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}
