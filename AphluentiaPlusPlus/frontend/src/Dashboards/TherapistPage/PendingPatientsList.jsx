import {React, useState} from 'react';
import './PatientRequests.css';
import Button from '@mui/material/Button';
import CancelIcon from '@mui/icons-material/Cancel';
import isBase64 from 'is-base64';
const PendingPatientsList = ({ patients, handlePatientReject }) => {
  return (
    <div
      className="patient-accepted"
      style={{
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}>
      <div style={{ width: '80%' }}>
        <h2>Pending Patients</h2>
        <div className="patient-requests-list" style={{ height: '300px', width: '100%', overflowY: 'auto' }}>
     
        <ul style={{ listStyle: 'none', padding: 0, border:'2px solid lightblue' }}>
          {
            patients.length == 0 ?<p>No Pending Requests</p>: <p></p>
          }
          {patients.map((patient, index) => (
            <li key={index} style={{ borderBottom: '1px solid lightgrey', padding: '1px' }}>            
              <div style={{ padding:"2%", display:'flex'}}>
                <div  style={{ maxWidth:'30%', padding:'1%' }}>
                  {isBase64(patient.profilePicture.split(",")[1]) ? (
                      <img
                        src={patient.profilePicture}
                        alt="Profile"
                        style={{ width: '100%', height: 'auto', maxWidth: '100px' }}
                        onClick={(e) => e.preventDefault()}
                      />
                    ):(
                      <p>Image Invalid</p>
                    )
                    }
                </div>
                <div>
                    <p><strong>Name:</strong> {patient.firstName} {patient.lastName}</p>
                    <p><strong>Email:</strong> {patient.email}</p>
                    <p><strong>Age:</strong> {patient.age}</p>
                    <p><strong>Condition:</strong> {patient.conditionName}</p>
                    
                </div>
              </div>
              <div style={{ display:'flex', maxWidth:'30%' }}>
                <div style={{padding:"2%" }}>
                <Button  variant="outlined" color="error"
                  startIcon={<CancelIcon />} 
                  onClick={() => handlePatientReject(patient.email)}>Cancel</Button>
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

export default PendingPatientsList;