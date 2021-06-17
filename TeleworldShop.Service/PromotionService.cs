using System;
using System.Collections.Generic;
using System.Linq;
using TeleworldShop.Common;
using TeleworldShop.Common.Exceptions;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Data.Repositories;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Service
{

    public interface IPromotionService
    {
        Promotion Add(Promotion Promotion);

        bool AddPromotionDetail(IEnumerable<PromotionDetail> roleGroups, int promotionId);

        void Update(Promotion promotion);

        void UpdateProductPromotionPrice(Promotion Promotion, List<PromotionDetail> promotionDetails);

        void UpdateProductPromotionPrice(int promotionId);

        Promotion GetById(int id);

        IEnumerable<Promotion> GetAll();

        IEnumerable<Promotion> GetAll(string keyword);

        void Save();

        //Promotion Delete(int id);

        //IEnumerable<Promotion> GetAll();

        //IEnumerable<Promotion> GetAll(string keyword);

        //Promotion GetById(int id);

        //IEnumerable<Promotion> Search(string keyword, int page, int pageSize, string sort, out int totalRow);


    }
    public class PromotionService : IPromotionService
    {
        private IPromotionRepository _promotionRepository;
        private IPromotionDetailRepository _promotionDetailRepository;
        private IProductRepository _productRepository;

        private IUnitOfWork _unitOfWork;

        public PromotionService(IPromotionRepository promotionRepository, IPromotionDetailRepository promotionDetailRepository,
            IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._promotionRepository = promotionRepository;
            this._promotionDetailRepository = promotionDetailRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public Promotion Add(Promotion Promotion)
        {
            try
            {
                Promotion promotion = _promotionRepository.Add(Promotion);
                _unitOfWork.Commit();

                return promotion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddPromotionDetail(IEnumerable<PromotionDetail> promotionDetails, int promotionId)
        {
            try
            {
                _promotionDetailRepository.DeleteMulti(x => x.PromotionId == promotionId);
                foreach (PromotionDetail promotionDetail in promotionDetails)
                {
                    _promotionDetailRepository.Add(promotionDetail);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateProductPromotionPrice(Promotion Promotion, List<PromotionDetail> promotionDetails)
        {
            try
            {
                if (Promotion.Type == 1)
                {
                    foreach (PromotionDetail promotionDetail in promotionDetails)
                    {
                        List<Product> products = _productRepository.GetMulti(
                            x => x.Status == true && x.CategoryId == promotionDetail.CategoryId && x.Price > Promotion.PromotionPrice
                        ).ToList();

                        foreach (Product product in products)
                        {
                            product.PromotionPrice = product.Price - Promotion.PromotionPrice;
                            _productRepository.Update(product);
                        }
                    }
                }
                else
                {
                    foreach (PromotionDetail promotionDetail in promotionDetails)
                    {
                        List<Product> products = _productRepository.GetMulti(
                            x => x.Status == true && x.CategoryId == promotionDetail.CategoryId
                        ).ToList();

                        foreach (Product product in products)
                        {
                            product.PromotionPrice = product.Price - (product.PromotionPrice * (Promotion.PromotionPrice / 100));
                            _productRepository.Update(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProductPromotionPrice(int promotionId)
        {
            try
            {
                List<PromotionDetail> promotionDetails = _promotionDetailRepository.GetMulti(x => x.PromotionId == promotionId).ToList();
                foreach (PromotionDetail promotionDetail in promotionDetails)
                {
                    List<Product> products = _productRepository.GetMulti(
                            x => x.Status == true && x.CategoryId == promotionDetail.CategoryId
                        ).ToList();

                    foreach (Product product in products)
                    {
                        product.PromotionPrice = null;
                        _productRepository.Update(product);
                    }
                }
            }
            catch
            {

            }
        }

        public Promotion GetById(int id)
        {
            return _promotionRepository.GetSingleById(id);
        }

        public void Update(Promotion promotion)
        {
            _promotionRepository.Update(promotion);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Promotion> GetAll()
        {
            return _promotionRepository.GetAll();
        }

        public IEnumerable<Promotion> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _promotionRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _promotionRepository.GetAll();
        }
    }
}
