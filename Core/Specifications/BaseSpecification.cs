using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria=criteria;
        }


        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;} = 
        new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }


        /// <summary>
        /// The function adds an include expression to a list.
        /// </summary>
        /// <param name="includeExpression">The includeExpression parameter is a lambda expression that
        /// specifies the navigation property to be included in the query. It takes a generic type T and
        /// returns an object.</param>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// The function "AddOrderBy" sets the "OrderBy" property to the specified expression.
        /// </summary>
        /// <param name="orderByExpression">The orderByExpression parameter is an expression that
        /// represents the property or properties by which the collection should be ordered. It is of
        /// type Expression<Func<T, object>>. The T represents the type of the elements in the
        /// collection. The expression should specify the property or properties to order by, and it
        /// should return</param>
         protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        /// <summary>
        /// The ApplyPaging function sets the skip and take values for paging and enables paging.
        /// </summary>
        /// <param name="skip">The "skip" parameter is used to specify the number of items to skip
        /// before starting to retrieve the items. It is typically used for implementing pagination,
        /// where you want to skip a certain number of items before retrieving the next set of
        /// items.</param>
        /// <param name="take">The "take" parameter specifies the number of items to retrieve or "take"
        /// from a collection or data source. It determines the maximum number of items that will be
        /// returned in a single page or query result.</param>
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

    }
}