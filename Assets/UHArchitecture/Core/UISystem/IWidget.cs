namespace UralHedgehog
{
    namespace UI
    {
        public interface IWidget
        {
            void Init();
            void Init(params object[] param);
            void Show();
            void Hide();
        }
    }
}