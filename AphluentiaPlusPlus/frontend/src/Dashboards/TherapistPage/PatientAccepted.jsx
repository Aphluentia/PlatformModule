import {React, useState} from 'react';
import './PatientList.css';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import isBase64 from 'is-base64';
import CancelIcon from '@mui/icons-material/Cancel';
import ArrowCircleRightIcon from '@mui/icons-material/ArrowCircleRight';
import { useNavigate } from 'react-router-dom';


const PatientAccepted = ({ patients, handlePatientReject}) => {
  const [list, setList] = useState(patients);
  const [query, setQuery] = useState('');
  if (patients != list && query ==='' ){
    setList(patients);
  }
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    const inputValue = e.target.value;
    setQuery(inputValue);
    let filteredList = patients.filter(item =>
      item.email.toLowerCase().includes(inputValue.toLowerCase()) ||
      item.firstName.toLowerCase().includes(inputValue.toLowerCase()) ||
      item.lastName.toLowerCase().includes(inputValue.toLowerCase()) ||
      item.age.toString().toLowerCase().includes(inputValue.toLowerCase()) ||
      item.conditionName.toLowerCase().includes(inputValue.toLowerCase())
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
        height: 300,
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <div style={{ width: '80%' }}>
        <h2>Current Patients</h2>
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
                <Button  variant="outlined" color="error"
                  startIcon={<CancelIcon />} 
                  onClick={() => handlePatientReject(patient.email)}>Remove</Button>
                </div>
                <div style={{padding:"2%" }}>
                <Button  variant="outlined" color="success"
                  startIcon={<ArrowCircleRightIcon />} 
                  onClick={() => navigate(patient.email)}>Visit</Button>
                </div>
              </div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default PatientAccepted;