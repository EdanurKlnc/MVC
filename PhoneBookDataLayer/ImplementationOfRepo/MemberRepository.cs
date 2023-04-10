using PhoneBookDataLayer.InterfaceOfRepo;
using PhoneBookEntityLayer.Entities;

namespace PhoneBookDataLayer.ImplementationOfRepo
{
    public class MemberRepository: Repository<Member, string>, IMemberRepository
    {
        public MemberRepository(MyContext context):base(context)
        {
            //Kalıtım aldığı atasındaki ctor 1 pareametre aldığı için , burayada ekledik.
            
        }
        //_context burada kullanılabilir. çÇünkü repositoryde  procted olarak yazıldı.

    }
}
