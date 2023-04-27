using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IDamageable
    {
        int m_Health { get; set; }

        void TakeDamage(int damage);
    }
}
