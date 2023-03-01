import React, { useEffect, useState } from 'react';
import axios from "axios";
import "./Home.css";

function Home() {

    const [name, setName] = useState();
    const [value, setValue] = useState();
    const [employees, setEmployees] = useState([])
    const [sums, setSums] = useState();

    
    //creating an employee
    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            const res = await axios.post("http://localhost:41478/List/CreateEmployee",
                {
                    name,
                    value
                });
            setEmployees([...employees, res.data]);
            setName("");
            setValue("");
            console.log("successfully saved employee");
        } catch (err) {
            console.log(err);
        }
    };

    //deleting an employee
    const handleDelete = async (name) => {
        try {
            await axios.delete(`http://localhost:41478/List/RemoveEmployee/${name}`);
            setEmployees(
                employees.filter(
                    (employee) => employee.name !== name)
            );
            console.log("successfully deleted employee")
        } catch (err) {
            console.log(err);
        }
    };

    //getting employees
    useEffect(() => {
        const getEmployees = async () => {
            const res = await axios.get("http://localhost:41478/List/GetEmployees");
            setEmployees(res.data);
        }
        getEmployees()
    }, [employees]);

    //when clicking button, this will increment the values
    const handleIncrement = async () => {
        try {
            await axios.get("http://localhost:41478/List/increment");
            console.log("increment operation successfully done")
        } catch (err) {
            console.log(err);
        }
    };

    //Modify value of an employee
    const [employeeName, setEmployeeName] = useState("");
    const [newValue, setNewValue] = useState("");

    const handleUpdate = async () => {
        try {
            const res = await axios.put("http://localhost:41478/List/PutEmployee", {
                name: employeeName,
                value: parseInt(newValue),
            });
            console.log("employee updated")
            setEmployees([...employees, res.data]);
            setEmployeeName("");
            setNewValue("");
        } catch (err) {
            console.log(err);
        }
    };

    //I could be able to fetch SUM(Value) values that are
    //bigger than or equal to 11171 by using POSTMAN and SQLite Online Editor.

    /*const getSums = async () => {
        try {
            await axios.get("http://localhost:41478/List/sum")
                .then(res => res.json())
                .then(
                    (results) => {
                        setSums([...results]);
                    }
                );

        } catch (err) {
            console.log(err);
        }
    }*/


    return (
        <div>
            <h1>Metricell Junior Task</h1>
            <div className="employee-operations">
                <h3>Add Employee</h3>
                <form className="createEmployeeForm" onSubmit={handleSubmit}>
                    <input
                        placeholder="Name"
                        required
                        onChange={(e) => setName(e.target.value)}
                        className="nameInput"
                    />
                    <input
                        placeholder="Value"
                        required
                        onChange={(e) => setValue(e.target.value)}
                        className="valueInput"
                    />
                    <button className="createBtn" type="submit">
                        Create Employee
                    </button>
                </form>
            </div>
            <div>
                <h1>Employee List</h1>
                <button onClick={handleIncrement} >Increase Employee Value</button>
                <form className="modifyValueForm" onSubmit={handleUpdate}>
                    <input
                        placeholder="Name of the employee"
                        required
                        type="text"
                        value={employeeName}
                        onChange={(e) => setEmployeeName(e.target.value)}
                    />
                    <input
                        placeholder="Value"
                        required
                        type="number"
                        value={newValue}
                        onChange={(e) => setNewValue(e.target.value)}
                    />
                    <button type="submit">Change Value</button>
                </form>
                <table className="employee-table">
                    <thead>
                        <tr>
                            <th>Employee Name</th>
                            <th>Employee Value</th>
                            <th>Remove</th>
                        </tr>
                    </thead>
                    <tbody>{
                        employees.map((employee) => {
                            return (
                                <tr>
                                    <td>{employee.name}</td>
                                    <td>{employee.value}</td>
                                    <th>
                                        <button onClick={() => handleDelete(employee.name)}>Remove</button>
                                    </th>
                                </tr>
                            );
                        })
                    }</tbody>
                </table>
            </div>

        </div>
    )
}

export default Home

