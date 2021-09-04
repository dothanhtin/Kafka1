using Confluent.Kafka;
using Core.IServices.IOrderServices;
using Kafka1.ViewModels;
using KafkaPubSub;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Enums;
using Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka1.Controllers
{
    [ApiController]
    [Route("api/core/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IProcessOrderServices _processingOrderService;
        public OrderController(ProducerConfig producerConfig, IProcessOrderServices processOrderService)
        {
            this._producerConfig = producerConfig;
            this._processingOrderService = processOrderService;
        }
        /// <summary>
        /// Create order service 
        /// Cause at the moment only have one topic and one event handle for create order service
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("createorder")]
        [HttpPut]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestViewModel orderRequest, CancellationToken cancellationToken)
        {
            string messageDefault = $"Your order is error. Please check again!";
            ApiResponse result = new ApiResponse((int)EnumExtension.ApiCodeResponse.error, messageDefault);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Serialize 
            var model = orderRequest.getObject();
            string serializedOrder = JsonConvert.SerializeObject(model);
            switch (KafkaConfig.Instance.status)
            {
                case (int)EnumExtension.KafkaStatus.enable:
                    //Send with Kafka
                    var producer = new ProducerWrapper(this._producerConfig, KafkaHelper.orderRequestTopic, model.orderId);
                    var res = await producer.writeMessage(serializedOrder);
                    if (!string.IsNullOrEmpty(res))
                    {
                        result.code = (int)EnumExtension.ApiCodeResponse.success;
                        result.message = $"Your order is in progress with id is {res}";
                        return Ok(result);
                    }
                    break;
                default:
                    //Send direction
                    var res1 = await _processingOrderService.CreateOrderAsync(model, cancellationToken);
                    if (!string.IsNullOrEmpty(res1))
                    {
                        result.code = (int)EnumExtension.ApiCodeResponse.success;
                        result.message = $"Your order is in progress with id is {res1}";
                        return Ok(result);
                    }
                    break;
            }
            return BadRequest(result);
        }
        /// <summary>
        /// update order
        /// Kaka has not built yet
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("updateorder")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderRequestViewModel orderRequest, CancellationToken cancellationToken)
        {
            string messageDefault = $"Your update is error. Please check again!";
            ApiResponse result = new ApiResponse((int)EnumExtension.ApiCodeResponse.error, messageDefault);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(orderRequest.orderId))
                return BadRequest(new ApiResponse((int)EnumExtension.ApiCodeResponse.error, "Parameters is invalid!"));
            //Serialize 
            var model = orderRequest.getUpdateObject();
            string serializedOrder = JsonConvert.SerializeObject(model);
            switch (KafkaConfig.Instance.status)
            {
                case (int)EnumExtension.KafkaStatus.enable:
                    //Send with Kafka
                    break;
                default:
                    //Send direction
                    var res1 = await _processingOrderService.UpdateOrderAsync(model, cancellationToken);
                    if (!string.IsNullOrEmpty(res1))
                    {
                        result.code = (int)EnumExtension.ApiCodeResponse.success;
                        result.message = $"Your order is updated with id is {res1}";
                        return Ok(result);
                    }
                    break;
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="deleteViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("deleteorder")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteViewModel deleteViewModel, CancellationToken cancellationToken)
        {
            string messageDefault = $"Your delete is error. Please check again!";
            ApiResponse result = new ApiResponse((int)EnumExtension.ApiCodeResponse.error, messageDefault);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(deleteViewModel.id))
                return BadRequest(new ApiResponse((int)EnumExtension.ApiCodeResponse.error, "Parameters is invalid!"));
            switch (KafkaConfig.Instance.status)
            {
                case (int)EnumExtension.KafkaStatus.enable:
                    //Send with Kafka
                    break;
                default:
                    //Send direction
                    var res1 = await _processingOrderService.DeleteOrderAsync(deleteViewModel.id, cancellationToken);
                    if (!string.IsNullOrEmpty(res1))
                    {
                        result.code = (int)EnumExtension.ApiCodeResponse.success;
                        result.message = $"Your order is deleted with id is {res1}";
                        return Ok(result);
                    }
                    break;
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("getorderbyid")]
        [HttpGet]
        public async Task<IActionResult> GetOrderById([FromQuery] string id, CancellationToken cancellationToken)
        {
            string messageDefault = $"Your order can not found. Please check again!";
            ApiResponse result = new ApiResponse((int)EnumExtension.ApiCodeResponse.error, messageDefault);
            if (string.IsNullOrEmpty(id))
                return BadRequest(result);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res1 = await _processingOrderService.GetOrderById(id, cancellationToken);
            if (res1 != null)
            {
                result.code = (int)EnumExtension.ApiCodeResponse.success;
                result.message = $"Your order is gotten successfully and it's id is {res1}";
                result.data = res1;
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("getallorders")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
            string messageDefault = $"Your orders can not found. Please check again!";
            ApiResponse result = new ApiResponse((int)EnumExtension.ApiCodeResponse.error, messageDefault);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res1 = await _processingOrderService.GetAllOrderAsync(cancellationToken);
            if (res1 != null)
            {
                result.code = (int)EnumExtension.ApiCodeResponse.success;
                result.message = $"Your order is gotten successfully";
                result.data = res1;
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
