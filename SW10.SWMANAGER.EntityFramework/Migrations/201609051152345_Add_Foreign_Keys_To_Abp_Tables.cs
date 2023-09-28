namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Add_Foreign_Keys_To_Abp_Tables : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("AbpUserOrganizationUnits", "UserId", "AbpUsers", "Id", cascadeDelete: false);
            AddForeignKey("AbpUserOrganizationUnits", "OrganizationUnitId", "AbpOrganizationUnits", "Id", cascadeDelete: false);
            AddForeignKey("AbpUserRoles", "RoleId", "AbpRoles", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("AbpUserRoles", "RoleId", "AbpRoles");
            DropForeignKey("AbpUserOrganizationUnits", "OrganizationUnitId", "AbpOrganizationUnits");
            DropForeignKey("AbpUserOrganizationUnits", "UserId", "AbpUsers");
        }
    }
}
