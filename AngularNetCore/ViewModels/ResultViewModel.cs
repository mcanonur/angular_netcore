namespace AngularNetCore.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel(bool isSuccess, object result)
        {
            this.IsSuccess = isSuccess;
            this.Result = result;
        }

        public bool IsSuccess { get; set; }
        public object Result { get; set; }
    }
}
