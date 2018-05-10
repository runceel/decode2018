using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeApp
{
    public class ValueHolder : BindableBase
    {
        public static ValueHolder Instance { get; } = new ValueHolder();

        private string _text;

        public string Text
        {
            get { return this._text; }
            set { this.SetProperty(ref this._text, value); }
        }

    }
}
