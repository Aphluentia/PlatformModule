import {React, useState, useEffect} from 'react';
import './PatientList.css';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import CancelIcon from '@mui/icons-material/Cancel';
import isBase64 from 'is-base64';

const PatientList = ({ patients, handlePatientRequest }) => {

  const [list, setList] = useState(patients);
  const [query, setQuery] = useState('');
  if (patients != list && query ==='' ){
    setList(patients);
  }

  const handleInputChange = (e) => {
    const inputValue = e.target.value;
    setQuery(inputValue);
    let filteredList = patients.filter(item =>
      item.email.toLowerCase().includes(inputValue.toLowerCase())
    );
    if (inputValue === '') {
     filteredList = patients;
    }
    
    setList(filteredList);
   
  };
  

  return (
    <div
      className="patient-accepted"
      style={{
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}>
      <div style={{ width: '80%' , 
        marginBottom:'5%',}}>
        <h2>Available Patients</h2>
        <div className="patient-accepted" style={{ height: '300px', width: '100%', overflowY: 'auto' }}>
        <input
          type="text"
          placeholder="Search..."
          value={query}
          onChange={handleInputChange}
        />
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {
            list.length == 0 ?<p>No Available Patients</p>: <p></p>
          }
          {list.map((patient, index) => (
            <li key={index} style={{ borderBottom: '1px solid lightgrey', padding: '10px', display: 'flex' }}>            
              <div style={{ width: '20%', marginRight:"2px"}}>
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
              {/* Second Column: Name, Email, Age, Condition Type */}
              <div style={{ width: '40%'}}>
                <div style={{padding:"2%" }}>
                  <strong>Name:</strong> {patient.firstName} {patient.lastName}
                </div>
                <div style={{padding:"2%" }}>
                  <strong>Email:</strong> {patient.email}
                </div>
              </div>
              <div style={{ width: '40%' }}>
                <div style={{padding:"2%" }}>
                  <strong>Age:</strong> {patient.age}
                </div>
                <div style={{padding:"2%" }}>
                  <strong>Condition Type:</strong> {patient.conditionName}
                </div>
              </div>
              {/* Third Column: Edit and Delete Buttons */}
              <div style={{ width: '20%' }}>
                <div style={{padding:"2%" }}>
                <Button  variant="contained" color="success"  
                  startIcon={<AddCircleOutlineIcon />} 
                  onClick={() => handlePatientRequest(patient.email)}>Request Patient</Button>
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

export default PatientList;