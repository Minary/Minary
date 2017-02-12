# Minary

Minary is a man-in-the-middle data packet rerouting and sniffing tool. It spoofs target systems that are connected to the same LAN 
as an attacker. Data packets sent from the target systems to the Minary system are captured and rerouted. Furthermore the content
of the captured data packets also is passed to all loaded Minary plugins to evaluate, manipulate and report the data on the GUI.

The mitm part for IPv4 networks is done with ARP poisoning. For IPv6 network the solution is not yet implemented.
