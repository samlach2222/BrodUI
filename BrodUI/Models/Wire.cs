using System.Windows.Media;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage a wire
    /// </summary>
    public class Wire
    {
        /// <summary>
        /// Getter and setter of the color
        /// </summary>
        public Brush Color { get; set; }

        /// <summary>
        /// Getter and setter of the number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Getter and setter of the type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Getter and setter of the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Getter and setter of the quantity
        /// </summary>
        public int Quantity { get; set; }

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
