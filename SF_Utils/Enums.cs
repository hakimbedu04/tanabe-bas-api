using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Utils
{
    public class Enums
    {
        public enum ResponseTypeActualVisit
        {
            [Description("Please input your visit product")]
            T0,
            [Description("Please input your visit product")]
            null_product,
            [Description("There is no record for filled criteria")]
            null_realized,
            [Description("Edit successful")]
            success_edit,
            [Description("Edit failed, there is problem with model")]
            error_edit,
            [Description("Request has been sent")]
            success_send,
            [Description("There is error on sending email notification")]
            send_limitation

        }
        public enum ResponseType
        {
            #region visit plan

            #region Success
            //default
            [Description("Request has been sent")]
            SuccessSend,
            [Description("Success")]
            Success,
            [Description("Delete Success")]
            SuccessDeleteDefault,
            //[Description("Sales Promotion has been realized and Email for realization approval request sent")]
            //SuccessRealization,
            [Description("Visit Realization has been successfuly saved")]
            SuccessRealization,
            [Description("Cancellation has been successfully submitted")]
            SuccessSubmitCancellation,
            [Description("Success send file topic to doctor")]
            SuccessSendFile,


            [Description("Add New Sales Promotion Plan Success")]
            SuccessAdd,
            [Description("Delete Sales Promotion Plan Success")]
            SuccessDelete,
            [Description("Delete Product Success")]
            SuccessDeleteProduct,
            [Description("Sales Promotion Plan has been updated")]
            SuccessUpdate,
            [Description("Budget allocation has been update successful")]
            SuccessUpdateBa,
            [Description("Visit Plan Date has been updated")]
            SuccessUpdatePlan,
            [Description("Feedback has been successfuly updated")]
            SuccessUpdateFeedback,
            [Description("Product has been updated")]
            SuccessUpdateProduct,
            [Description("Additional realization has been successfuly saved")]
            SuccessAdditionalRealization,
            #endregion

            #region validation
            [Description("System can't generate scheme because User plan has been already created in current month with selected doctors")]
            UserAlreadyPlanned,
            [Description("Cannot send request, because you still have some doctors who has not planned visit yet")]
            DoctorUnplanedVisit,
            [Description("Cannot send request, because you still have a planned visit that has no product planning")]
            DoctorUnplanedProduct,
            [Description("Cannot send request, because you still have some doctors who has not planned user yet")]
            DoctorUnplanedSales,
            [Description("verification plan request is limited to 3 times per month")]
            SendLimitation,
            [Description("Cannot send request, you still have one day less than minimum visits")]
            LessDoctor,
            [Description("You still have some doctors are tied to the realization visit in the previous month that has not been verified by your Manager")]
            PrevMonthUnverificatedReal,
            [Description("You've exceeded the maximum limit doctor visits on that day")]
            ReachMaximumDoctor,
            [Description("There is already plan  doctor with name you selected on that day")]
            AlreadyPlannedDoctor,
            [Description("There is already plan doctor with name you selected on that week")]
            AlreadyPlannedDoctorWeek,
            [Description("It looks like you've done the planning visit on this month")]
            AlreadyPlannedVisit,
            [Description("Leave code can not be paired with a doctor code")]
            PairedCode,
            [Description("There is no visit planned on selected dates")]
            nullPlanned,
            [Description("Can't add product unless you choose the doctor")]
            NullDoctorPlan,
            [Description("Can't add product unless you choose the date and doctors")]
            NullDatePlan,
            [Description("Can't add or delete product,because you have made SP Plan related to this product, if you want to add or delete the product, please delete plan SP for first")]
            SpAlreadyPlan,
            [Description("You have entered the product with the same code before but with different percentage")]
            PercentNotMatch,

            [Description("SP2 has been created for the doctor you selected on that date")]
            ExistsSp,

            [Description("The total percentage you specified on product details, is not even 100 percent")]
            ProductIncomplete,
            [Description("Can't add product unless you choose the doctor")]
            NullDoctorPlanAddVisitProductFlag,
            [Description("Please input the products first")]
            NullProduct,
            [Description("The total percentage you specified on product details, is not even 100 percent")]
            ProductIncompleteSp,
            [Description("Visit id or Dr Code null")]
            NullInputs,
            [Description("The topic is already entered")]
            TopicExists,
            [Description("Please add signature or submit deviation")]
            NullSign,
            [Description("Please choose budget")]
            NullBudgetRealization,
            [Description("Please choose realize button")]
            NullButtonRealization,
            #endregion

            #region failure

            [Description("Oops! Unauthorize user or expired session. please ty again later")]
            UnauthorizeUser,
            [Description("Oops! there is unexpected error, please contact your administrator")]
            InternalServerError,
            [Description("There is error on submitting data")]
            SaveError,
            [Description("Add New Sales Promotion Plan Erorr. Please Try Again")]
            ErrorAdd,
            [Description("Delete Sales Promotion Plan Erorr. Please Try Again")]
            ErrorDelete,
            [Description("Update Sales Promotion Plan Erorr. Please Try Again")]
            ErrorUpdate,
            [Description("Insert Failed, Query error please call your administrator")]
            QueryError,
            [Description("There is an error on submitting cancellation process")]
            ErrorSubmitCancellation,
            [Description("Your realization amount is 0")]
            RealValueNull,
            [Description("Error send email!")]
            EmailError,
            #endregion

            #region notifications

            #endregion





            #endregion


            #region Visit Realization

            #region success

            #endregion

            #region validation
            [Description("Cannot process this realization, because you have not entered supporting documents")]
            RealizationNullAttachment,
            [Description("Cannot process this realization, because you have not entered feedback for some of topic for this visit")]
            RealizationNullFeedback,
            [Description("Cannot process this realization, because you have not entered any topic for this visit")]
            RealizationNullTopic,
            #endregion

            #region failure
            #endregion

            #endregion

            #region SP Realization
            [Description("Sales Promotion has been realized and Email for realization approval request sent")]
            SuccessSpRealization,
            [Description("Realization Sales Promotion  Erorr. Please Try Again")]
            ErrorSpRealization,
            #endregion

            #region Visit Associated
            [Description("The associated Visit ID id not found")]
            ErrorVisitAssociatedIdNotFound,
            [Description("You are not authorized to make a changes to this item")]
            ErrorVisitAssociatedNotAuthorized,
            #endregion
        }

        public enum ActionSource
        {
            UserActual,
        }

        public enum ExcelTemplate
        {
            Visit_Plan,
            Visit_Realization,
            Visit_Actual,
            Visit_History,

            User_Plan,
            User_Realization,
            User_Actual,
            User_History,

            SP_Plan,
            SP_Actual,
            SP_Realization,
            SP_Report,
        }
    }
}
