namespace PowerBank_AQA_UITestingCore.Models.Mediator
{
    public interface IMediator
    {
        void Execute(Action action, int? timeout = null);

        object Execute<TResult>(Func<TResult> action, int? timeout = null);

        object Wait<TResult>(Func<TResult> action, int? timeout = null);
    }
}
