using System;

namespace Symbioz.ORM
{
    public class TableAttribute : Attribute
    {
        public string tableName;
        public bool catchAll;
        public short readingOrder;
        public short addingOrder;
        public bool letInUpdateField;

        public TableAttribute(string tableName,  bool catchAll = true, short order = 99,short readingOrder = -1, bool letInUpdateField = false)
        {
            this.tableName = tableName;
            this.catchAll = catchAll;
            this.readingOrder = readingOrder;
            this.addingOrder = readingOrder;
            this.letInUpdateField = letInUpdateField;
        }
    }
}
