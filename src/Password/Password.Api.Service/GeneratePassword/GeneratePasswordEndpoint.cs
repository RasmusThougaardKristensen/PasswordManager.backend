﻿using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Password.ApplicationServices.PasswordGenerator;
using Swashbuckle.AspNetCore.Annotations;

namespace PasswordManager.Password.Api.Service.GeneratePassword
{
    public sealed class GeneratePasswordEndpoint : EndpointBaseAsync.WithRequest<int>.WithActionResult<GeneratePasswordResponse>
    {
        private readonly IGeneratePasswordService _generatePasswordService;

        public GeneratePasswordEndpoint(IGeneratePasswordService generatePasswordService)
        {
            _generatePasswordService = generatePasswordService;
        }

        [HttpGet("api/generate/password")]
        [ProducesResponseType(typeof(GeneratePasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
        Summary = "Generates Password",
        Description = "Generates a password",
        OperationId = "GeneratePassword",
        Tags = new[] { "Password" })
        ]

        public override async Task<ActionResult<GeneratePasswordResponse>> HandleAsync([FromQuery] int passwordLength, CancellationToken cancellationToken)
        {
            try
            {
                var password = new GeneratePasswordResponse(await _generatePasswordService.GeneratePassword(passwordLength));
                return Ok(password);
            }
            catch (Exception ex)
            {
                return Problem(title: "Password could not be generated",
                               detail: $"Password could not be generated: {ex.Message}",
                               statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
