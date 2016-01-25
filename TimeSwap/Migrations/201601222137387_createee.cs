namespace TimeSwap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CARGO",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CARGO1 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RECURSO",
                c => new
                    {
                        MATRICULA = c.String(nullable: false, maxLength: 128),
                        NOME = c.String(),
                        CARGOID = c.Int(nullable: false),
                        TELEFONE = c.String(),
                        CPF = c.String(),
                        LOGIN = c.String(),
                        SENHA = c.String(),
                        HOMEMHORA = c.Double(),
                        EMAIL = c.String(),
                    })
                .PrimaryKey(t => t.MATRICULA)
                .ForeignKey("dbo.CARGO", t => t.CARGOID)
                .Index(t => t.CARGOID);
            
            CreateTable(
                "dbo.PROJETO",
                c => new
                    {
                        CODIGO = c.String(nullable: false, maxLength: 128),
                        NOME = c.String(),
                        GERENTEID = c.String(),
                        CAMINHO = c.String(),
                        RECURSO_MATRICULA = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CODIGO)
                .ForeignKey("dbo.RECURSO", t => t.RECURSO_MATRICULA)
                .Index(t => t.RECURSO_MATRICULA);
            
            CreateTable(
                "dbo.FASE",
                c => new
                    {
                        FASEID = c.Int(nullable: false, identity: true),
                        PROJETOID = c.String(),
                        DESCRICAO = c.String(),
                        INICIOREAL = c.DateTime(),
                        FIMREAL = c.DateTime(),
                        INICIOBASE = c.DateTime(),
                        FIMBASE = c.DateTime(),
                        DURACAO = c.Double(),
                        PROJETO_CODIGO = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FASEID)
                .ForeignKey("dbo.PROJETO", t => t.PROJETO_CODIGO)
                .Index(t => t.PROJETO_CODIGO);
            
            CreateTable(
                "dbo.TAREFA",
                c => new
                    {
                        TAREFAID = c.String(nullable: false, maxLength: 128),
                        PROJETOID = c.String(),
                        FASEID = c.Int(nullable: false),
                        DESCRICAO = c.String(),
                        TIPO = c.Int(),
                        LINHA = c.Int(),
                        LINHADEP = c.Int(),
                        DURACAO = c.Double(),
                        INICIOREAL = c.DateTime(),
                        FIMREAL = c.DateTime(),
                        INICIOBASE = c.DateTime(),
                        FIMBASE = c.DateTime(),
                        PORCENTAGEM = c.Double(),
                    })
                .PrimaryKey(t => t.TAREFAID)
                .ForeignKey("dbo.FASE", t => t.FASEID)
                .Index(t => t.FASEID);
            
            CreateTable(
                "dbo.RECURSOTAREFA",
                c => new
                    {
                        RECURSOID = c.String(nullable: false, maxLength: 128),
                        TAREFAID = c.String(nullable: false, maxLength: 128),
                        FASEID = c.Int(nullable: false),
                        PROJETOID = c.String(),
                        DATAINICIO = c.DateTime(nullable: false),
                        DATAFIM = c.DateTime(),
                        STATUS = c.Int(),
                        RECURSO_MATRICULA = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RECURSOID, t.TAREFAID })
                .ForeignKey("dbo.RECURSO", t => t.RECURSO_MATRICULA)
                .ForeignKey("dbo.TAREFA", t => t.TAREFAID)
                .Index(t => t.TAREFAID)
                .Index(t => t.RECURSO_MATRICULA);
            
            CreateTable(
                "dbo.RecursoTarefaExecutado",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        recursoId = c.String(maxLength: 128),
                        projetoId = c.String(),
                        faseId = c.Int(nullable: false),
                        tarefaId = c.String(maxLength: 128),
                        DataHoraInicio = c.DateTime(nullable: false),
                        DataInicio = c.DateTime(nullable: false),
                        DataHoraFim = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.RECURSOTAREFA", t => new { t.recursoId, t.tarefaId })
                .Index(t => new { t.recursoId, t.tarefaId });
            
            CreateTable(
                "dbo.RECURSOHABILIDADE",
                c => new
                    {
                        RECURSOID = c.String(nullable: false, maxLength: 128),
                        HABILIDADEID = c.Int(nullable: false),
                        NIVEL = c.Int(),
                        RECURSO_MATRICULA = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RECURSOID)
                .ForeignKey("dbo.HABILIDADE", t => t.HABILIDADEID)
                .ForeignKey("dbo.RECURSO", t => t.RECURSO_MATRICULA)
                .Index(t => t.HABILIDADEID)
                .Index(t => t.RECURSO_MATRICULA);
            
            CreateTable(
                "dbo.HABILIDADE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DESCRICAO = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RECURSOHABILIDADE", "RECURSO_MATRICULA", "dbo.RECURSO");
            DropForeignKey("dbo.RECURSOHABILIDADE", "HABILIDADEID", "dbo.HABILIDADE");
            DropForeignKey("dbo.PROJETO", "RECURSO_MATRICULA", "dbo.RECURSO");
            DropForeignKey("dbo.RECURSOTAREFA", "TAREFAID", "dbo.TAREFA");
            DropForeignKey("dbo.RecursoTarefaExecutado", new[] { "recursoId", "tarefaId" }, "dbo.RECURSOTAREFA");
            DropForeignKey("dbo.RECURSOTAREFA", "RECURSO_MATRICULA", "dbo.RECURSO");
            DropForeignKey("dbo.TAREFA", "FASEID", "dbo.FASE");
            DropForeignKey("dbo.FASE", "PROJETO_CODIGO", "dbo.PROJETO");
            DropForeignKey("dbo.RECURSO", "CARGOID", "dbo.CARGO");
            DropIndex("dbo.RECURSOHABILIDADE", new[] { "RECURSO_MATRICULA" });
            DropIndex("dbo.RECURSOHABILIDADE", new[] { "HABILIDADEID" });
            DropIndex("dbo.RecursoTarefaExecutado", new[] { "recursoId", "tarefaId" });
            DropIndex("dbo.RECURSOTAREFA", new[] { "RECURSO_MATRICULA" });
            DropIndex("dbo.RECURSOTAREFA", new[] { "TAREFAID" });
            DropIndex("dbo.TAREFA", new[] { "FASEID" });
            DropIndex("dbo.FASE", new[] { "PROJETO_CODIGO" });
            DropIndex("dbo.PROJETO", new[] { "RECURSO_MATRICULA" });
            DropIndex("dbo.RECURSO", new[] { "CARGOID" });
            DropTable("dbo.HABILIDADE");
            DropTable("dbo.RECURSOHABILIDADE");
            DropTable("dbo.RecursoTarefaExecutado");
            DropTable("dbo.RECURSOTAREFA");
            DropTable("dbo.TAREFA");
            DropTable("dbo.FASE");
            DropTable("dbo.PROJETO");
            DropTable("dbo.RECURSO");
            DropTable("dbo.CARGO");
        }
    }
}
