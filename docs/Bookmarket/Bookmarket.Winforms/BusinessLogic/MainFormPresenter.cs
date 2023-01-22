using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarket.Winforms.BusinessLogic
{
    public class MainFormPresenter
    {
        private readonly IMainFormView mainFormView;

        public MainFormPresenter(IMainFormView mainFormView)
        {
            this.mainFormView = mainFormView;
        }
    }
}
