import { fetchData } from '../../api/api.js';


export async function renderSuppliers() {
  const container = document.getElementById('main-content');

  try {
    const suppliers = await fetchData('suppliers');
    
    if (!suppliers.length) {
      container.innerHTML = `<p>No suppliers found.</p>`;
      return;
    }

    container.innerHTML = `
      <h2>Suppliers</h2>
      <ul>
        ${suppliers.map(s => `
          <li class="supplier-item">
            <h3>${s.name}</h3>
            <p><strong>Address:</strong> ${s.address}</p>
            <p><strong>Contact Person:</strong> ${s.contactPerson}</p>
            <p><strong>Phone Number:</strong> ${s.phoneNumber}</p>
            <p><strong>Email:</strong> <a href="mailto:${s.email}">${s.email}</a></p>
          </li>
        `).join('')}
      </ul>
    `;
  } catch (err) {
    console.error('Error fetching suppliers:', err);
    container.innerHTML = `<p>Error loading suppliers. Please try again later.</p>`;
  }
}

  
