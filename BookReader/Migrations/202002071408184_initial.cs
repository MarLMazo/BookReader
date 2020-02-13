namespace BookReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        BookTitle = c.String(),
                        BookDescrp = c.String(),
                        BookAuthor = c.String(),
                        BookPublish = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookId);
            
            CreateTable(
                "dbo.Readers",
                c => new
                    {
                        ReaderId = c.Int(nullable: false, identity: true),
                        ReaderFname = c.String(),
                        ReaderLname = c.String(),
                        ReaderAddress = c.String(),
                        ReaderPhone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReaderId);
            
            CreateTable(
                "dbo.BookReaders",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        ReaderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookId, t.ReaderId })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Readers", t => t.ReaderId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.ReaderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookReaders", "ReaderId", "dbo.Readers");
            DropForeignKey("dbo.BookReaders", "BookId", "dbo.Books");
            DropIndex("dbo.BookReaders", new[] { "ReaderId" });
            DropIndex("dbo.BookReaders", new[] { "BookId" });
            DropTable("dbo.BookReaders");
            DropTable("dbo.Readers");
            DropTable("dbo.Books");
        }
    }
}
