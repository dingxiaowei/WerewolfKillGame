namespace Hotfix.Manager
{
    public class ManagerBase<T> : IManager where T : IManager, new()
    {
        protected static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new T();
                }
                return m_Instance;
            }
        }

        protected ManagerBase()
        {

        }

        public virtual void Init()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void OnApplicationQuit()
        {

        }
    }
}
