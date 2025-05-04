import { fetchData } from '../../api/api.js';


export async function renderCustomers() {
  const container = document.getElementById('main-content');



  try {
    const customers = await fetchData('customers');
    
    if (!customers.length) {
      container.innerHTML = `<p>No customers found.</p>`;
      return;
    }

    container.innerHTML = `
      <h2>Customers</h2>
      <ul>
        ${customers.map(c => `
          <li class="customer-item">
            <h3>${c.name}</h3>
            <p><strong>Phone:</strong> ${c.phone}</p>
            <p><strong>Email:</strong> <a href="mailto:${c.email}">${c.email}</a></p>
            <p><strong>Contact Person:</strong> ${c.contactPerson}</p>
            <p><strong>Delivery Address:</strong> ${c.deliveryAddress}</p>
            <p><strong>Invoice Address:</strong> ${c.invoiceAddress}</p>
          </li>
        `).join('')}
      </ul>
    `;
  } catch (err) {
    console.error('Error fetching customers:', err);
    container.innerHTML = `<p>Error loading customers. Please try again later.</p>`;
  }
}

