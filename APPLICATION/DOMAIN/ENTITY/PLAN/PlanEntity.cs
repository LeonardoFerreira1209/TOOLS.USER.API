using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.ROLE;

namespace APPLICATION.DOMAIN.ENTITY.PLAN
{
    public class PlanEntity : BaseEntity
    {
        /// <summary>
        /// Nome do plano
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Descrição do plano
        /// </summary>
        public string PlanDescription { get; set; }

        /// <summary>
        /// Valor do plano
        /// </summary>
        public double PlanCost { get; set; }

        /// <summary>
        /// Permissões do plano
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public virtual RoleEntity Role { get; set; }

        /// <summary>
        /// Total de meses do plano
        /// </summary>
        public int TotalMonthsPlan { get; set; }

    }
}
