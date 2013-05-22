﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "MC")]   
#endif
#if !SILVERLIGHT
    [Serializable]
#endif
    #endregion
    public class MethodCallExpressionNode : ExpressionNode<MethodCallExpression>
    {
        public MethodCallExpressionNode() { }

        public MethodCallExpressionNode(INodeFactory factory, MethodCallExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "A")]
#endif
        #endregion
        public ExpressionNodeList Arguments { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MethodInfoNode Method { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "O")]
#endif
        #endregion
        public ExpressionNode Object { get; set; }

        protected override void Initialize(MethodCallExpression expression)
        {
            this.Arguments = new ExpressionNodeList(this.Factory, expression.Arguments);
            this.Method = new MethodInfoNode(this.Factory, expression.Method);
            this.Object = this.Factory.Create(expression.Object);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            Expression objectExpression = null;
            if (this.Object != null)
                objectExpression = this.Object.ToExpression(context);

            return Expression.Call(objectExpression, this.Method.ToMemberInfo(context), this.Arguments.GetExpressions(context).ToArray());
        }
    }
}
