using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constant;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;


        public ProductManager(IProductDal productDal, ICategoryService categoryDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategory(product.CategoryId), CheckProductNameUnique(product.ProductName), CheckCategoryNumber());
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            // Koşullar burada yazılacak. Örnek yetkisi var mı?
            if (DateTime.Now.Hour == 16)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new DataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id), true, "Products successfully get");
        }

        public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
        {
            return new DataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max), true, "Product successfully get");

        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId), "Product successfully get");
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 16)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), "Product detail successfully get");
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            if (CheckIfProductCountOfCategory(product.CategoryId).Success)
            {
                _productDal.Add(product);
                return new SuccessResult(Messages.ProductAdded);
            }
            return new ErrorResult();

        }

        private IResult CheckIfProductCountOfCategory(int categoryId)
        {
            var dataList = GetAllByCategoryId(categoryId);
            if (dataList.Data.Count > 10)
            {
                return new ErrorResult(Messages.MaxCategory);
            }
            return new SuccessResult();
        }
        private IResult CheckProductNameUnique(string productName)
        {
            var result = _productDal.Get(p => p.ProductName == productName);
            if (result != null)
            {
                return new ErrorResult(Messages.ProductNameInvalid);
            }
            return new SuccessResult();

        }
        private IResult CheckCategoryNumber()
        {
            var result = _categoryService.GetAll().Data.Count;
            if (result >15)
            {
                return new ErrorResult(Messages.CategoryOutofLimit);
            }
            return new SuccessResult();
        }
    }
}
