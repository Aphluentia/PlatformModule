import {React, useState} from 'react';
import './Css/TherapistList.css';
import Button from '@mui/material/Button';
import isBase64 from 'is-base64';
import CancelIcon from '@mui/icons-material/Cancel';


const AcceptedTherapist = ({ therapists, handleTherapistReject}) => {
  return (
    <div className="therapist-list-section-container" style={{maxWith:"20%"}}>
    <div style={{ width: '90%', marginBottom:'5%', }}>
      <h2>Assigned Therapists</h2>
      <div style={{ overflowY: 'auto' }}>
   
      <ul className="therapist-list">
        {
          therapists.length == 0 ?<p>No Therapist Assigned</p>: <p></p>
        }
        {therapists.map((therapist, index) => (
             <li key={index} style={{ margin:'1px', border: '1px solid lightgrey', display: 'flex'}}>            
             <div style={{ padding:"2%", display:'flex'}}>
               <div  style={{ width: '70px',   padding:'1%'}}>
                 {isBase64(therapist.profilePicture.split(",")[1]) ? (
                     <img
                       src={therapist.profilePicture}
                       alt="Profile"
                       style={{ height: 'auto',  maxWidth: '70px'}}
                       onClick={(e) => e.preventDefault()}
                     />
                   ):(
                     <p>Image Invalid</p>
                   )
                   }
               </div>
               <div style={{ maxWidth:'60%',   padding:'1%'}}>
                 <p><strong>{therapist.firstName} {therapist.lastName}</strong> </p>
                 <p>{therapist.email}</p>
                 <p><strong>Age:</strong> {therapist.age}</p>
                 <p><strong>Description:</strong> {therapist.description}</p>
                 <p> <strong>Credentials:</strong> {therapist.credentials}</p>
               </div>
             </div>
             <div style={{ display:'flex', maxWidth:'30%' }}>
               <div style={{padding:"2%" }}>
                 <Button  variant="outlined" color="error"
                   startIcon={<CancelIcon />} 
                   onClick={() => handleTherapistReject(therapist.email)}>Reject</Button>
               </div>
             </div>
           </li>
        ))}
      </ul>
    </div>
    </div>
  </div>
  );
};

export default AcceptedTherapist;