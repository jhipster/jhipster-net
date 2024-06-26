 using JHipsterNet.Core.Pagination.Extensions;

namespace JHipsterNet.Core.Pagination {
    public static class PageableConstants {
        public static readonly IPageable UnPaged = new UnPaged();
    }

    public interface IPageable {
        bool IsPaged { get; }
        int PageNumber { get; }
        int PageSize { get; }
        int Offset { get; }
        Sort Sort { get; }
        IPageable Next { get; }
        IPageable PreviousOrFirst { get; }

        IPageable First { get; }

        bool HasPrevious { get; }
    }

    internal class UnPaged : IPageable {
        public bool IsPaged => false;
        public Sort Sort => Sort.Unsorted;
        public IPageable Next => this;
        public IPageable PreviousOrFirst => this;

        public IPageable First => this;

        public bool HasPrevious => false;

        public int PageNumber => 0;
        public int PageSize => PageableBinderConfig.DefaultMaxPageSize;

        public int Offset => 0;
    }
}
