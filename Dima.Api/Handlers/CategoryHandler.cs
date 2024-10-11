using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
	public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
	{
		try
		{
			var category = new Category
			{
				UserId = request.UserId,
				Title = request.Title,
				Description = request.Description,
			};

			await context.Categories.AddAsync(category);
			await context.SaveChangesAsync();

			return new Response<Category?>(category, 201, "Category created");
		}
		catch
		{
			return new Response<Category?>(null, 500, "Category not created");
		}
	}

	public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
	{
		try
		{
			var category = await context
				.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

			if (category == null) return new Response<Category?>(null, 404, "Category not found");

			category.Title = request.Title;
			category.Description = request.Description;

			context.Categories.Update(category);
			await context.SaveChangesAsync();

			return new Response<Category?>(category, message: "Category updated");
		}
		catch
		{
			return new Response<Category?>(null, 500, "Category not updated");
		}
	}

	public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
	{
		try
		{
			var category = await context
				.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

			if (category == null) return new Response<Category?>(null, 404, "Category not found");

			context.Categories.Remove(category);
			await context.SaveChangesAsync();

			return new Response<Category?>(category, message: "Category deleted");
		}
		catch
		{
			return new Response<Category?>(null, 500, "Category not deleted");
		}
	}

	public Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
	{
		throw new NotImplementedException();
	}

	public Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
	{
		throw new NotImplementedException();
	}
}
