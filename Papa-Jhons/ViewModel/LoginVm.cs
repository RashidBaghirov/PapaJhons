using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Papa_Jhons.ViewModel
{
    public class LoginVm
    {
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
