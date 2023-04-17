using System.Windows.Media;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage a wire
    /// </summary>
    public class Wire
    {
        /// <summary>
        /// Color of the wire, RGB color who are used in the image
        /// </summary>
        private Brush _color;

        /// <summary>
        /// Number of the wire, DMC number, VàC number, etc.
        /// </summary>
        private int _number;

        /// <summary>
        /// Type of the wire, DMC, VàC, etc.
        /// </summary>
        private string _type;

        /// <summary>
        /// Name of the wire, DMC name, VàC name, etc.
        /// </summary>
        private string _name;

        /// <summary>
        /// Quantity of the wire
        /// </summary>
        private int _quantity;

        /// <summary>
        /// Getter and setter of the color
        /// </summary>
        public Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Getter and setter of the number
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /// <summary>
        /// Getter and setter of the type
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Getter and setter of the name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Getter and setter of the quantity
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="color">Color of the wire</param>
        /// <param name="number">Number of the wire</param>
        /// <param name="type">Type of the wire</param>
        /// <param name="name">Name of the wire</param>
        /// <param name="quantity">Quantity of the wire</param>
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
