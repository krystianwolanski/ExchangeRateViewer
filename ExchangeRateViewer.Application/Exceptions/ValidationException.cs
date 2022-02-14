using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateViewer.Application.Exceptions
{
    public class ValidationException : ExchangeRateViewerException
    {
        public IDictionary<string, string[]> Errors { get; }
        private IEnumerable<ValidationFailure> _failures;

        public override string Message 
            => "\n" + string.Join(Environment.NewLine, _failures.Select(f => "--- " + f.ErrorMessage));

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            _failures = failures;
        }

    }
}
