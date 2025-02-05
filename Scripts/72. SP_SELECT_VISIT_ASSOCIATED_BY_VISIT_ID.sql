USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_VISIT_ASSOCIATED_BY_VISIT_ID]    Script Date: 08/09/2023 16.23.17 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[SP_SELECT_VISIT_ASSOCIATED_BY_VISIT_ID]
@visit_id varchar(15) 

AS
 select va.associate_id, CONCAT('Your planned visit for Doctor  ',dr.dr_name,' on ',FORMAT(vs.visit_date_plan,'dd/MM/yyyy'),' has been associated by your ',rep_initiator.rep_position ,' ',rep_initiator.rep_name,
 ' with the actual visit date on ', FORMAT(vp.visit_date_plan,'dd/MM/yyyy'), '. Please provide your confirmation for this claim.' )
 va_notif, va.visit_id visit_id_initiator, vp.dr_code dr_code_initiator, dr.dr_name , vp.visit_date_plan visit_date_plan_initiator,  vs.visit_date_plan visit_date_plan_invited, vp.rep_id rep_id_initiator, vp.visit_date_realization_saved, vs.visit_id visit_id_invited,vs.rep_id rep_id_invited, va.associate_status,
 rep_initiator.rep_name initiator_name,  rep_initiator.rep_email initiator_email, rep_invited.rep_name invited_name, rep_invited.rep_email invited_email
 from t_visit_associate va LEFT JOIN t_visit vp ON va.visit_id = vp.visit_id
 LEFT JOIN t_visit vs ON va.visit_id_associated = vs.visit_id
 LEFT JOIN m_doctor dr ON vp.dr_code = dr.dr_code
 LEFT JOIN m_rep rep_initiator ON vp.rep_id = rep_initiator.rep_id 
  LEFT JOIN m_rep rep_invited ON vs.rep_id = rep_invited.rep_id 
 WHERE vp.visit_id = @visit_id



	
GO
