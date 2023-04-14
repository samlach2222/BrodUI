using System.Windows.Media;

namespace BrodUI.Models
{
    public class Wire
    {
        private Brush _color; // RGB color who are used in the image
        private int _number; // TYPE number (DMC number, VàC number, etc.)
        private string _type; // DMC, VàC, etc.
        private string _name; // TYPE name (DMC name, VàC name, etc.)
        private int _quantity; // quantity of the wire

        public Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public Wire(Brush color, int number, string type, string name, int quantity)
        {
            Color = color;
            Number = number;
            Type = type;
            Name = name;
            Quantity = quantity;
        }
    }
}
