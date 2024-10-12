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

	public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
	{
		try
		{
			var category = await context
				.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

			if (category == null) return new Response<Category?>(null, 404, "Category not found");

			return new Response<Category?>(category);
		}
		catch
		{
			return new Response<Category?>(null, 500, "Something went wrong");
		}
	}

	public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
	{
		var query = context.Categories.AsNoTracking().Where(c => c.UserId == request.UserId);

		try
		{
			var categories = await query
				.OrderBy(c => c.Title)
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync();

			if (categories == null) return new PagedResponse<List<Category>?>(null, 404, "Categories not found");

			var count = await query.CountAsync();

			return new PagedResponse<List<Category>?>(categories, count, request.PageNumber, request.PageSize);
		}
		catch
		{
			return new PagedResponse<List<Category>?>(null, 500, "Something went wrong");
		}
	}
}
