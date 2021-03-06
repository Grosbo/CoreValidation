﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreValidation.Errors
{
    public sealed class ErrorsCollection : IErrorsCollection
    {
        private readonly List<IError> _errors = new List<IError>();

        private readonly Dictionary<string, ErrorsCollection> _memberErrors = new Dictionary<string, ErrorsCollection>();

        public static IErrorsCollection Empty { get; } = new ErrorsCollection();

        public bool IsEmpty => !Errors.Any() && !Members.Any();


        public IReadOnlyCollection<IError> Errors => _errors;

        public IReadOnlyDictionary<string, IErrorsCollection> Members => _memberErrors.ToDictionary(pair => pair.Key, pair => pair.Value as IErrorsCollection);

        public void Include(IErrorsCollection errorsCollection)
        {
            if (errorsCollection == null)
            {
                throw new ArgumentNullException(nameof(errorsCollection));
            }

            if (errorsCollection.IsEmpty)
            {
                return;
            }

            foreach (var member in errorsCollection.Members)
            {
                AddError(member.Key, member.Value);
            }

            foreach (var error in errorsCollection.Errors)
            {
                AddError(error);
            }
        }

        public void AddError(IError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            _errors.Add(error);
        }

        public void AddError(string memberName, IError error)
        {
            var errorsCollection = new ErrorsCollection();
            errorsCollection.AddError(error);

            AddError(memberName, errorsCollection);
        }

        public void AddError(string memberName, IErrorsCollection errorsCollection)
        {
            if (memberName == null)
            {
                throw new ArgumentNullException(nameof(memberName));
            }

            if (memberName == string.Empty)
            {
                throw new ArgumentException("Empty string", nameof(memberName));
            }

            if (errorsCollection == null)
            {
                throw new ArgumentNullException(nameof(errorsCollection));
            }

            if (errorsCollection.IsEmpty)
            {
                return;
            }

            if (!_memberErrors.ContainsKey(memberName))
            {
                _memberErrors.Add(memberName, new ErrorsCollection());
            }

            _memberErrors[memberName].Include(errorsCollection);
        }

        public static IErrorsCollection WithSingleOrNull(IError error)
        {
            if (error == null)
            {
                return null;
            }

            var errorCollection = new ErrorsCollection();
            errorCollection.AddError(error);

            return errorCollection;
        }
    }
}