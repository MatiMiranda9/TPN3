using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace DemoBlazorMovil.Helpers
{
    public class ObjectGraphDataAnnotationsValidator : ComponentBase
    {
        private ValidationMessageStore? messageStore;

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(ObjectGraphDataAnnotationsValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ObjectGraphDataAnnotationsValidator)} " +
                    "inside an EditForm."
                );
            }

            messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) => ValidateModel(CurrentEditContext);
            CurrentEditContext.OnFieldChanged += (s, e) => ValidateModel(CurrentEditContext);
        }

        private void ValidateModel(EditContext editContext)
        {
            if (messageStore == null) return;

            messageStore.Clear();
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(editContext.Model);
            Validator.TryValidateObject(editContext.Model, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                if (validationResult == ValidationResult.Success) continue;

                foreach (var memberName in validationResult.MemberNames)
                {
                    messageStore.Add(editContext.Field(memberName), validationResult.ErrorMessage!);
                }
            }

            editContext.NotifyValidationStateChanged();
        }
    }
}
