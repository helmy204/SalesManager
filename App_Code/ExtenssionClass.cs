﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Reflection;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Runtime.Serialization;
/// <summary>
/// Summary description for ExtenssionClass
/// </summary>
namespace System.Data.ElkhateebDynamicLinq// plz dont change it
{
    public static partial class ExtenssionClass 
    {

        #region Constructor
        static ExtenssionClass()
        {


        }
       #endregion
        //2 member field
        #region object
     
     //   static DataContext db = null;// i will be check it another time.
       static bool Transaction=false;
        #endregion
        //1 method
       // get datacontext object
        #region get datacontext object
       static void   DC<T>(T entity)where T :class 
        {

            if (HttpContext.Current.Session["db"] == null)
            {
                HttpContext.Current.Session["db"] = (DataContext)entity.GetType().Assembly.GetTypes().Where(p => p.GetCustomAttributes(typeof(DatabaseAttribute), true).Length == 1).Where(p => p.GetProperties().Where(z => z.PropertyType == typeof(Table<T>)).Count() > 0 || p.GetMethods().Where(m => m.ReturnType == typeof(ISingleResult<T>)).Count() > 0).Select(p => p.Assembly.CreateInstance(p.FullName)).First();
              
            }

            if (HttpContext.Current.Session["db"] == null) throw new Exception("This Type not Mapped to  DataContext");

        }
       #endregion
        //6 method
        #region Transaction
        static void OpenConnection()
        {
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            if (db != null)
            {
                if (db.Connection.State == ConnectionState.Closed)
                    db.Connection.Open();
            }
        }
        
        public static void BeginTransaction<T>(this T entity)where T :class 
        {
            DC<T>(entity);           
            OpenConnection();
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            if (db != null)
            {
                db.Transaction = db.Connection.BeginTransaction(IsolationLevel.ReadCommitted);

              Transaction = true;
            }
            else
            {
                throw new Exception(" DataContext object is null");
            }
        }
       public static void Commit()
        {
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            if (db != null)
            {
                db.Transaction.Commit();
                CloseConnection();
            }
        }
       public static void Rollback()
       {
           DataContext db = HttpContext.Current.Session["db"] as DataContext;
           if (db != null)
           {
               db.Transaction.Rollback();
               CloseConnection();
           }
        }
        public static void EndTransaction<T>(this T entity) 
        {
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            if (db != null)
            {
                try
                {

                    Commit();

                }
                catch (Exception ex)
                {
                    Rollback();
                    throw ex;
                }

            }
           
            
        }
        static void CloseConnection()
        {
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            if (db != null)
            {
                if (db.Connection.State == ConnectionState.Open)
                    db.Connection.Close();
                db = null;
               Transaction = false;
            }
           
        }
        #endregion
        //4 method
        #region Begin Modify data
        /// <summary>
        /// return 0 if fail or >0 if successed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object Insert<T>(this T entity) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            try
            {
                db.GetTable<T>().InsertOnSubmit(entity);
                SubmitChanges();
                return entity.GetType().GetProperties().Where(p => ((ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute), true)[0]).IsPrimaryKey == true).Select(p => p.GetValue(entity, null)).First();
            }
            catch (Exception ex) { if (Transaction) { Rollback(); CloseConnection(); } throw ex; return 0; }

        }

        public static void  InsertAll<T>(this IEnumerable<T> entities) where T : class,new ()
        {
            T entity = new T();
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            try
            {
                db.GetTable<T>().InsertAllOnSubmit(entities);
                SubmitChanges();
               
            }
            catch (Exception ex) { if (Transaction) { Rollback(); CloseConnection(); } throw ex;  }

        }
        /// <summary>
        /// return false if error occur and raise error,else true if succesed
        /// example for Expression emp.Update <![CDATA[<Employee>]]>(x => x.EmployeeID == emp.EmployeeID);  
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="query">example for Expression ( System.Linq.Expressions.Expression'<![CDATA[<Func<Employee , bool>>]]> expr ;
        ///expr = x => x.EmployeeID  ==emp.EmployeeID )</param>
        /// <returns> true or false </returns>
        public static bool Update<T>(this T entity, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            try
            {
                object propertyValue = null;
             
                T entityFromDB =db.GetTable <T>().SingleOrDefault(predicat);

                if (null == entityFromDB)
                    throw new NullReferenceException("Query Supplied to Get entity from DB is invalid, NULL value returned");

                PropertyInfo[] properties = entityFromDB.GetType().GetProperties().Where (p=>p.GetCustomAttributes (typeof(ColumnAttribute),true ).Length ==1).ToArray ();

                foreach (PropertyInfo property in properties)
                {
                    propertyValue = null;

                    if (null != property.GetSetMethod())
                    {
                        PropertyInfo entityProperty = entity.GetType().GetProperty(property.Name);

                        if (entityProperty.PropertyType.BaseType == Type.GetType("System.ValueType") || entityProperty.PropertyType == Type.GetType("System.String"))
                        {
                            propertyValue = entity.GetType().GetProperty(property.Name).GetValue(entity, null);
                          
                        }

                        if (null != propertyValue)
                            property.SetValue(entityFromDB, propertyValue, null);
                    }
                }

                SubmitChanges();
            }
            catch (Exception ex) { if (Transaction) { Rollback(); CloseConnection(); } throw ex; return false; }
            return true;
        }

        public static bool Delete<T>(this T entity, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            try
            {
                db.GetTable<T>().DeleteOnSubmit(db.GetTable<T>().SingleOrDefault(predicat));
                SubmitChanges();
            }
            catch (Exception ex) { if (Transaction) { Rollback(); CloseConnection(); } throw ex; return false; }
            return true;
        }
        public static void DeleteAll<T>(this IEnumerable<T> entities) where T : class,new()
        {
            T entity = new T();
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            try
            {
                db.GetTable<T>().DeleteAllOnSubmit(entities);
                SubmitChanges();

            }
            catch (Exception ex) { if (Transaction) { Rollback(); CloseConnection(); } throw ex ; }

        }
        static void SubmitChanges()
        {
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            if (db != null)
            {
                db.SubmitChanges();
                if (Transaction == false)
                {

                    db = null;
                }
            }
        }
        #endregion
        //5 method
        #region Select data
        public static IQueryable<T> Get<T>(this T entity) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
                // DataContext dbCopy = db.CloneDataContext();if(!Transaction) db = null; return dbCopy.GetTable<T>().AsQueryable<T>(); 
                return db.GetTable<T>().AsQueryable<T>(); 
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }

        public static IQueryable<BT> Get<T, BT>(this T entity, Expression<Func<T, BT>> SelectExpression)
            where T : class
            where BT : struct
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
              //  DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null; return dbCopy.GetTable<T>().AsQueryable<T>().Select(SelectExpression);
                return db.GetTable<T>().AsQueryable<T>().Select(SelectExpression);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        public static IQueryable<string> Get<T>(this T entity, Expression<Func<T, string>> SelectExpression)
            where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            { 
               // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null; return dbCopy.GetTable<T>().AsQueryable<T>().Select(SelectExpression);
                return db.GetTable<T>().AsQueryable<T>().Select(SelectExpression);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }

        public static IQueryable<BT> Get<T, BT>(this T entity, Expression<Func<T, bool>> FilterExpression, Expression<Func<T, BT>> SelectExpression)
            where T : class
            where BT : struct
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
              //  DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null; return dbCopy.GetTable<T>().AsQueryable<T>().Where(FilterExpression).Select(SelectExpression);
                return db.GetTable<T>().AsQueryable<T>().Where(FilterExpression).Select(SelectExpression);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        public static IQueryable<string> Get<T>(this T entity, Expression<Func<T, bool>> FilterExpression, Expression<Func<T, string>> SelectExpression)
            where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
              //  DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
                return db.GetTable<T>().AsQueryable<T>().Where(FilterExpression).Select(SelectExpression);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        #endregion
        //3 method
        #region Filter data With Paging
        public static T Single<T>(this T entity, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
               // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
                return db.GetTable<T>().AsQueryable<T>().Where(predicat).Single<T>();
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        public static IQueryable<T> Filter<T>(this T entity, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
               // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
                return db.GetTable<T>().AsQueryable<T>().Where(predicat);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        public static IQueryable<T> Filter<T>(this T entity, int startRows, int maxRows) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
               // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
                return db.GetTable<T>().AsQueryable<T>().Skip(startRows).Take(maxRows);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        public static IQueryable<T> Filter<T>(this T entity, Expression<Func<T, bool>> predicat, int startRows, int maxRows) where T : class
        {
            DC<T>(entity); 
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
              object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return db.GetTable<T>().AsQueryable<T>().Where(predicat).Skip(startRows).Take(maxRows);
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        
        }
      
        #endregion
        //2 method
        #region ValueTypeMethod
        public static int RowsCount<T>(this T entity) where T :class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
            object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
               // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
                return db.GetTable<T>().Count<T>();
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
        }
        public static int RowsCount<T>(this T entity, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity); 
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
              object[] Tables = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (Tables.Length == 1)
            {
           //  DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
             return db.GetTable<T>().Where(predicat).Count<T>();
            }
            else throw new Exception("This Type not Mapped as a Table In DataBase");
       
        }
        #endregion
        //5 method
        #region Select data from Stored Proceedure
        //ISingleResul Get method for invok method which Mapping to call  stored Proceedure  with any parameter
        public static IQueryable <T> Get<T>(this T entity,object [] parameter) where T:class 
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>();
           
        }
        public static IQueryable<BT> Get<T, BT>(this T entity, object[] parameter, Expression<Func<T, BT>> SelectExpression)
            where T : class
            where BT : struct
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Select(SelectExpression);

        }
        public static IQueryable<string > Get<T>(this T entity, object[] parameter, Expression<Func<T, string >> SelectExpression)
            where T : class
           
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Select(SelectExpression);

        }

        public static IQueryable<BT> Get<T, BT>(this T entity, object[] parameter, Expression<Func<T, bool>> FilterExpression, Expression<Func<T, BT>> SelectExpression)
            where T : class
            where BT : struct
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Where(FilterExpression).Select(SelectExpression);

        }
        public static IQueryable<string > Get<T>(this T entity, object[] parameter, Expression<Func<T, bool>> FilterExpression, Expression<Func<T, string >> SelectExpression)
            where T : class
           
        {
            DC<T>(entity);
             DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Where(FilterExpression).Select(SelectExpression);

        }
        #endregion
        //3 method
        #region Filter data From Stored Proceedure With Paging
        public static T Single<T>(this T entity, object[] parameter, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Where(predicat).Single<T>();

        }
        public static IQueryable<T> Filter<T>(this T entity, object[] parameter, Expression<Func<T, bool>> predicat) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
           // DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Where(predicat);

        }
        public static IQueryable<T> Filter<T>(this T entity, object[] parameter, int startRows, int maxRows) where T : class
        {
            DC<T>(entity);
            DataContext db = HttpContext.Current.Session["db"] as DataContext;
          //  DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Skip(startRows).Take(maxRows);

        }
        public static IQueryable<T> Filter<T>(this T entity, object[] parameter, Expression<Func<T, bool>> predicat, int startRows, int maxRows) where T : class
        {
            DC<T>(entity);
             DataContext db = HttpContext.Current.Session["db"] as DataContext;
         //  DataContext dbCopy = db.CloneDataContext(); if (!Transaction) db = null;
            return ((ISingleResult<T>)db.GetType().GetMethods().Single(m => m.ReturnType == typeof(ISingleResult<T>)).Invoke(db, parameter)).AsQueryable<T>().Where(predicat).Skip(startRows).Take(maxRows);

        }
     
        #endregion
        //2 method
        #region ValueTypeMethod
        public static int RowsCount<T>(this T entity, object[] parameter) where T : class
        {
          return   Get<T>(entity , parameter).Count ();
        }
        public static int RowsCount<T>(this T entity, object[] parameter, Expression<Func<T, bool>> predicat) where T : class
        {
            return Get<T>(entity, parameter).Where (predicat ).Count();
        }
        #endregion
        //3 method
        #region DateTable
         
        ///copy structure of any dataSource
        public static DataTable DataSourceClone<T>(this IEnumerable<T> entity)
        {
           DataTable dtReturn = new DataTable();
            
            // column names 
            PropertyInfo[] oProps = null;

            if (entity  == null) return dtReturn;

            foreach (T rec in entity)
            {
                // Use reflection to get property names, to create table, Only first time, others   will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
            }
           return dtReturn ;
        }
        ///convert list to datetable
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(Nullable<>) || prop.PropertyType.IsGenericType == true)
                {
                    tb.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]); continue;
                }
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
       
        /// copy structure of any entity and convert ti datatable
        public static DataTable EntityClone<T>(this T entity)
        {
            var tb = new DataTable(typeof(T).Name);
            //this line is very usful
           // PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(Nullable<>) || prop.PropertyType.IsGenericType == true)
                { 
                    tb.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]); continue; 
                }
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

           

            return tb;
        }
        // Create DataPaging
     
     
        #endregion

        #region In And Not In 
        public static IQueryable<T> In<T>(this IQueryable<T> source,
                            IQueryable<T> checkAgainst)
        {
            return from s in source
                   where checkAgainst.Contains(s)
                   select s;
        }

        public static IQueryable<T> NotIn<T>(this IQueryable<T> source,
                                          IQueryable<T> checkAgainst)
        {
            return from s in source
                   where !checkAgainst.Contains(s)
                   select s;
        }
        #endregion

        #region we will look on it in aontherday
        //public static IQueryable<T> GetDataPaging<T>(this IEnumerable<T> entity, int RowsCount, int PageIndex, int PageSize)
        //{
        //    int startrow = PageSize * PageIndex;
        //    int lastrow = startrow + PageSize;


        //    return entity.Skip(startrow).Take(lastrow).AsQueryable<T>();


        //}
        //public static DataTable GetDataPaging<T>(this T entity, int PageIndex, int PageSize)where T:class 
        //{
        //    int startrow = PageSize * PageIndex;
        //    int lastrow = startrow + PageSize;
        //    int rowcount=db.GetTable<T>().Count();
        //    DataTable dt = new DataTable();
        //    dt = db.GetTable <T>().CustomClone();
        //    for (int i = 0; i <= rowcount; i++)
        //    {
        //        if (i >= startrow && i <= lastrow)
        //        {
        //            dt.Merge(db.GetTable<T>().Skip(startrow).Take(lastrow).ConvertToDataTable());
        //            i = lastrow;
        //        }
        //        else
        //        {
        //            DataRow dr = dt.NewRow();
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    return dt;
        //}
        //public static int GetDataPagingCount<T>(this IEnumerable<T> entity)
        //{
        //    return entity.Count();
        //}
        // Convert type (var)any datasource to DataTable
        //public static DataTable ConvertToDataTable<T>(this  IEnumerable<T> varlist) 
        //{
        //    DataTable dtReturn = new DataTable();

        //    // column names 
        //    PropertyInfo[] oProps = null;

        //    if (varlist == null) return dtReturn;

        //    foreach (T rec in varlist)
        //    {
        //        // Use reflection to get property names, to create table, Only first time, others   will follow 
        //        if (oProps == null)
        //        {
        //            oProps = ((Type)rec.GetType()).GetProperties();
        //            foreach (PropertyInfo pi in oProps)
        //            {
        //                Type colType = pi.PropertyType;

        //                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()== typeof(Nullable<>)))
        //                {
        //                    colType = colType.GetGenericArguments()[0];
        //                }

        //                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        //            }
        //        }

        //        DataRow dr = dtReturn.NewRow();

        //        foreach (PropertyInfo pi in oProps)
        //        {
        //            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
        //        }

        //        dtReturn.Rows.Add(dr);
        //    }
        //    return dtReturn;
        //}
        #endregion

        public static T CloneBySerializer<T>(this T source) where T : class
        {
            var obj = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            using (var stream = new System.IO.MemoryStream())
            {
                obj.WriteObject(stream, source);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)obj.ReadObject(stream);
            }
        }
        public static T CloneByReflection<T>(this T source)
        {
            var clone = (T)Activator.CreateInstance(typeof(T));
            var cols = typeof(T).GetProperties().Select(p => new { Prop = p, Attr = (ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute), true).SingleOrDefault() }).Where(p => p.Attr != null && !p.Attr.IsDbGenerated);
            foreach (var col in cols)
                col.Prop.SetValue(clone, col.Prop.GetValue(source, null), null); return clone;
        }
     
       
    }
}
