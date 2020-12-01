
namespace _2048
{
    public class Tile
    {
        public Tile()
        {
            Value = 0;
            Blocked = false;
        }

        public int Value { get; set; }
        public bool Blocked { get; set; }
    }
}
