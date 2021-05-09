using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs.Pagination
{
    public class EmployeesPaginationQuery
    {
        private string _filterColumn;
        private string _searchPhrase;
        private string _sortBy;
        public string FilterColumn 
        {
            get => _filterColumn;
            set => _filterColumn = string.IsNullOrEmpty(value) ? value : value.ToUpper();
        }
        public string SearchPhrase 
        {
            get => _searchPhrase;
            set => _searchPhrase = string.IsNullOrEmpty(value) ? value : value.ToUpper();
        }
        [Required]
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public string SortBy 
        {
            get => _sortBy;
            set => _sortBy = string.IsNullOrEmpty(value) ? value : value.ToUpper();
        }
    }
}
