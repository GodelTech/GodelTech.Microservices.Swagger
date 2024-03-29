﻿using System;
using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Swagger.Demo.Models.Fake;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Swagger.Demo.Controllers;

[Authorize]
[Route("fakes")]
[ApiController]
public class FakeController : ControllerBase
{
    private static readonly IReadOnlyList<FakeModel> Items = new List<FakeModel>
    {
        new FakeModel(),
        new FakeModel
        {
            Id = 1,
            Title = "Test Title"
        }
    };

    [HttpGet]
    [ProducesResponseType(typeof(IList<FakeModel>), StatusCodes.Status200OK)]
    public IActionResult GetList()
    {
        return Ok(Items);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FakeModel), StatusCodes.Status200OK)]
    public IActionResult Get(int id)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [Authorize("add")]
    [SwaggerSecurity("fake.add")]
    [HttpPost]
    [ProducesResponseType(typeof(FakeModel), StatusCodes.Status201Created)]
    public IActionResult Post([FromBody] FakePostModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var item = new FakeModel
        {
            Id = Items.Max(x => x.Id) + 1,
            Title = model.Title
        };

        return CreatedAtAction(
            nameof(Get),
            new { id = item.Id },
            item
        );
    }

    [Authorize("edit")]
    [SwaggerSecurity("fake.edit")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Put(int id, FakePutModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (id != model.Id)
        {
            return BadRequest();
        }

        var item = Items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize("delete")]
    [SwaggerSecurity("fake.delete")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete(int id)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);

        // delete functional here
        var result = item != null;

        if (!result)
        {
            return NotFound();
        }

        return Ok();
    }
}
