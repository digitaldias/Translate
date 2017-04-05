using Moq;
using StructureMap.AutoMocking.Moq;

namespace Translate.CrossCutting
{
    public class TestsFor<TEntity> where TEntity : class
    {
        protected TEntity Instance { get; set; }
        protected MoqAutoMocker<TEntity> AutoMocker { get; set; }

        public TestsFor()
        {
            AutoMocker = new MoqAutoMocker<TEntity>();

            Instance = AutoMocker.ClassUnderTest;
        }


        public Mock<TContract> GetMockFor<TContract>() where TContract : class
        {
            return Mock.Get(AutoMocker.Get<TContract>());
        }
    }
}
