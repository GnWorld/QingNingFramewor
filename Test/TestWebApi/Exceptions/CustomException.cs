namespace TestWebApi.Exceptions
{
    public class CustomException<T> : Exception where T : class
    {


        /// <summary>
        /// 模型信息
        /// </summary>
        public T Model { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        public CustomException(T model, string message) : base(message)
        {
            Model = model;
        }


    }

    public static class ExceptionExtension
    {
        public static void Throw<T>(T model, string message) where T : class
        {
            throw new CustomException<T>(model, message);
        }

    }

}
