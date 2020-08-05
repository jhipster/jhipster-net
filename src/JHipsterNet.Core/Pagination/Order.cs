
namespace JHipsterNet.Core.Pagination {
    public class Order {
        public bool IgnoreCase { get; set; }

        public Order(Direction direction, string property)
        {
            Direction = direction;
            Property = property;
        }
        public Direction Direction { get; }
        public string Property { get; }
    }

    public enum Direction {
        Asc,
        Desc
    }

    public static class DirectionExtensions {
        public static bool IsAscending(this Direction direction)
        {
            return direction.Equals(Direction.Asc);
        }

        public static bool IsDescending(this Direction direction)
        {
            return direction.Equals(Direction.Desc);
        }
    }
}
